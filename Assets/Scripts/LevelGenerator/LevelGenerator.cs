using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject terreno; //El modelo 3d del terreno (tiene que tener un rigidbody sin gravedad ni kinetico)
    private Vector3 terrenoSize; //Lo sacamos a partir del modelo 3d
    private List<MeshRenderer> terrenoList; //Lista de Sizes
    [SerializeField] private List<GameObject> TerrenoPool; //La pool de objetos para el terreno
    [SerializeField] private int poolSize; //El tamaño que ponemos nosotros que va a tener la pool
    [SerializeField] private int velocidadTerreno; //La velocidad a la que va el terreno (la velocidad de la nave)
    private GameObject prevTerreno; //Una referencia al un terreno cualquiera (para usarlo sin tener que inicializar un objeto)
    [SerializeField] private List<GameObject> terrenosActivos; //La referencia a los terrenos que estén activos

    private GameObject primerTerreno; //el ultimo terreno que esta (para poder poner el siguiente)
    [SerializeField]private int terrenosIniciales; //Cuantos terrenos va a haber a la vez en pantalla)
    private float distanciaUltimoTerrenoPlayer; //A que distancia debe desaparecer el terreno

    [SerializeField] private GameObject playerRef;//Referencia al jugador
    [SerializeField] private float distTierra;//Como de bajo tiene que estar el mapa
    void Start()
    {
        //Instanciar la object Pool
        TerrenoPool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(terreno);
            tmp.SetActive(false);
            TerrenoPool.Add(tmp);
        }
        //Pillar el tamaño de los objetos de la pool
        terrenoList= GetTerreno().GetComponentsInChildren<MeshRenderer>().ToList();
        var bounds = terrenoList[0].bounds;
        foreach(MeshRenderer mr in terrenoList)
        {
            bounds.Encapsulate(mr.bounds);
        }
        terrenoSize = bounds.size;

        //Referencia a los objetos de la pool que estén activos
        terrenosActivos = new List<GameObject>();

        //Crear los terrenos iniciales
        float mitad = (terrenosIniciales / 2);

        for (int i = 0; i < terrenosIniciales; i++)//Inicializar los terrenos que salgan al principio
        {
            prevTerreno = GetTerreno();
            prevTerreno.transform.position = new Vector3(terrenoSize.x*(i-mitad), distTierra, playerRef.transform.position.z);//Supongo que terrenos iniciales es impar
            prevTerreno.GetComponent<Rigidbody>().velocity = new Vector3(-velocidadTerreno, 0, 0);
            prevTerreno.SetActive(true);
            terrenosActivos.Add(prevTerreno);
        }
        //El último terreno es el primero(si lo contamos como desde atras)
        primerTerreno = prevTerreno;
        //Calculamos esta distancia (va a ser siempre un valor fijo a partir de este punto)
        distanciaUltimoTerrenoPlayer = terrenoSize.x * mitad;
    }
    
    void Update()
    {
        if(terrenosActivos[0].transform.position.x < -distanciaUltimoTerrenoPlayer)//Si el terreno se salio se desactiva y vuelve a la pool)
        {
            terrenosActivos[0].SetActive(false);
            terrenosActivos.RemoveAt(0);
            SpawnTerreno(); //Sacamos uno nuevo en el lugar del que ha desaparecido
        }
    }

    public GameObject GetTerreno() //Para pillar objetos de la pool (sacado de la documentación de Unity)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!TerrenoPool[i].activeInHierarchy)
            {
                return TerrenoPool[i];
            }
        }
        return null;
    }

    private void SpawnTerreno()//Sacar un terreno al principio del todo
    {
        prevTerreno = GetTerreno();
        prevTerreno.transform.position = new Vector3(primerTerreno.transform.position.x + terrenoSize.x, distTierra, playerRef.transform.position.z);
        prevTerreno.GetComponent<Rigidbody>().velocity = new Vector3(-velocidadTerreno, 0, 0);
        prevTerreno.SetActive(true);
        terrenosActivos.Add(prevTerreno);
        primerTerreno = prevTerreno;
    }
}
