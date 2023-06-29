using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerRef; //La referencia al jugador
    public float playerLife = 100f; //La vida del jugador


    [Header("Enemies")] //Esto depende de como haga pablo lo de los enemigos

    [SerializeField]private GameObject flyingEnemyRef;    //La referencia al enemigo (volador)
    [SerializeField] private int flyingPoolSize; //El tamaño que ponemos nosotros que va a tener la pool (voladores)
    [SerializeField] private int flyingEnemiesOnScreen; //Cuantos enemigos va a haber a la vez en pantalla(voladores)
    private List<GameObject> flyingEnemyPool; //La pool de enemigos voladores
    private List<GameObject> flyingEnemiesActivos; //La referencia a los enemigos que estén activos


    [SerializeField] private GameObject landEnemyRef;
    [SerializeField] private int landPoolSize; //El tamaño que ponemos nosotros que va a tener la pool (tierra)
    [SerializeField] private int landEnemiesOnScreen; //Cuantos enemigos va a haber a la vez en pantalla(tierra)
    private List<GameObject> landEnemyPool; //La pool de enemigos voladores
    private List<GameObject> landEnemiesActivos; //La referencia a los enemigos que estén activos

    private GameObject enemy; //Enemigo genérico para hacer las instancias;

    [Header("UI")]
    //Si está activado algún menú que pausa el juego
    public bool isPaused = false;

    //El canvas de todos los elementos a la vez de la UI ingame
    public GameObject canvasUI_Ingame;
    //El canvas del GameOver
    public GameObject canvasGameOver;
    //El canvas del GameOverVictory
    public GameObject canvasGameOverVictory;
    //El canvas del menu de pausa
    public GameObject canvasPauseMenu;

    [Header("Game variables")]
    //El tiempo que dura la partida
    [SerializeField] private float remainingTime;
    //Los enemigos que has matado
    public int enemyKills = 0;
    //Donde esta el agua
    public int landDistance;

    private void Start()
    {
        //Deshacer los cambios en caso de que terminemos una partida y le demos a jugar otra vez
        Time.timeScale = 1; //Reanudamos por si acaso el timeScale
        Random.InitState(System.DateTime.Now.Millisecond);
        
        //Instanciar la object Pool (voladores)
        flyingEnemyPool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < flyingPoolSize; i++)
        {
            tmp = Instantiate(flyingEnemyRef);
            tmp.SetActive(false);
            flyingEnemyPool.Add(tmp);
        }
        
        //Instanciar la object Pool (tierra)
        landEnemyPool = new List<GameObject>();

        for (int i = 0; i < landPoolSize; i++)
        {
            tmp = Instantiate(landEnemyRef);
            tmp.SetActive(false);
            landEnemyPool.Add(tmp);
        }

        //Referencia a los objetos de la pool que estén activos
        landEnemiesActivos = new List<GameObject>();
        flyingEnemiesActivos = new List<GameObject>();

        //Crear los enemigos iniciales
        for (int i = 0; i < flyingEnemiesOnScreen; i++)//Inicializar los terrenos que salgan al principio
        {
            SpawnEnemy("Flying");
        }
        for (int i = 0; i < landEnemiesOnScreen; i++)//Inicializar los terrenos que salgan al principio
        {
            SpawnEnemy("Land");
        }

    }
    
    private void Update()
    {
        if(remainingTime > 0)//Si no se ha acabado el tiempo sigue la partida
        {
            remainingTime = remainingTime - Time.deltaTime;
            DisplayTime(remainingTime);
        }
        else //Si se ha acabado el tiempo (condición de victoria)
        {
            GameOverVictory();
        }
    }
    

    private void GameOver() //Función que pasa cuando pierdes
    {
        isPaused = true; //Se indica que se para un menú
        //Ponemos el timeScale al 0 para que las cosas que dependan del tiempo no se actualicen
        Time.timeScale = 0;
        canvasGameOver.SetActive(true); //Activamos el canvas del GameOver
    }

    private void GameOverVictory() //Función que pasa cuando ganas
    {
        isPaused = true; //Se indica que se para un menú
        //Ponemos el timeScale al 0 para que las cosas que dependan del tiempo no se actualicen
        Time.timeScale = 0;

        int previousHighScore = GetHighScore(); //Pillamos la antigua puntuación
        int currentScore = CalculateHighScore(); //Calculamos la puntuación actual

        if(currentScore > previousHighScore) //Si tenemos ahora mejor puntuación la actualizamos
        {
            SetHighScore(currentScore);
        }

        //Actualizar UI de victoria

        canvasGameOverVictory.SetActive(true);//Activamos el canvas del GameOverVictory
    }

    public void ActivatePauseMenu()
    {
        isPaused = true; //Se indica que se para un menú
        Time.timeScale = 0; //Se paran los updates
        canvasUI_Ingame.SetActive(false);
        canvasPauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false; //Se indica que se reanuda
        Time.timeScale = 1; //Se paran los updates
        canvasUI_Ingame.SetActive(true);
        canvasPauseMenu.SetActive(false);
    }

    private void DisplayTime(float timeToDisplay) //Función para pasar de float a formato de minutos y segundos
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DecreasePLayerLife(float damage)
    {
        playerLife -= damage; //Le restamos el daño

        if(playerLife <= 0)//Si el jugador se muere
        {
            GameOver();
        }

        //Actualizar el UI
    }

    public void DestroyEnemy(GameObject enemy, string type)
    {
        //Destruir el enemigo
        if(type == "Flying")
        {
            enemy.SetActive(false);
            flyingEnemiesActivos.Remove(enemy);
            SpawnEnemy("Flying");
        }else if(type == "Land")
        {
            enemy.SetActive(false);
            landEnemiesActivos.Remove(enemy);
            SpawnEnemy("Land");
        }
        enemyKills += 1;
    }

    private int CalculateHighScore() //Es una función por si se quiere cambiar
    {
        return enemyKills;
    }

    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("HighScoreModified", 1); //Marcamos que hemos modificado el valor
        PlayerPrefs.SetInt("HighScore", score); //Actualizamos el valor
    }

    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScoreModified") && PlayerPrefs.GetInt("HighScoreModified") == 1)//Si se ha modificado la puntuacion
        {
            return PlayerPrefs.GetInt("HighScore"); //La devuelve
        }
        else
        {
            return 0; //Si no se ha modificado devuelve 0
        }
    }

    public GameObject GetFlyingEnemy() //Para pillar objetos de la pool (sacado de la documentación de Unity)
    {
        for (int i = 0; i < flyingPoolSize; i++)
        {
            if (!flyingEnemyPool[i].activeInHierarchy)
            {
                return flyingEnemyPool[i];
            }
        }
        return null;
    }

    public GameObject GetLandEnemy() //Para pillar objetos de la pool (sacado de la documentación de Unity)
    {
        for (int i = 0; i < landPoolSize; i++)
        {
            if (!landEnemyPool[i].activeInHierarchy)
            {
                return landEnemyPool[i];
            }
        }
        return null;
    }

    public void SpawnEnemy (string type)
    {
        int x = Random.Range(-140, 140);
        if (type == "Flying")
        {
            int y = Random.Range(-8, 2);
            int z = Random.Range(-10,10);
            enemy = GetFlyingEnemy();
            enemy.transform.position = new Vector3(x, y , z);

            enemy.transform.LookAt(playerRef.transform.position, Vector3.up);

            enemy.SetActive(true);
            flyingEnemiesActivos.Add(enemy);
        }
        else if(type == "Land")
        {
            int z = Random.Range(-2, 2);
            enemy = GetLandEnemy();
            enemy.transform.position = new Vector3(x, landDistance, z);

            enemy.transform.LookAt(new Vector3(playerRef.transform.position.x, landDistance, playerRef.transform.position.z), Vector3.up);

            enemy.SetActive(true);
            landEnemiesActivos.Add(enemy);
        }

        EnemyController cont = enemy.GetComponent<EnemyController>();
        cont.vida = 100;
        cont.ChangePos(enemy.transform.position);
    }

}
