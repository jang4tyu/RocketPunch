using UnityEngine;
using System.Collections;

public class Immortal : MonoBehaviour
{
	protected void SetImmortal()
	{
		string name = gameObject.name;
		gameObject.name += "dummy";
		GameObject obj = GameObject.Find( name );
		if (obj != null)
		{
			DestroyImmediate( gameObject );
		}
		else
		{
			gameObject.name = name;
			DontDestroyOnLoad( gameObject );
		}
	}

	public void Awake()
	{
		//base.Awake();
		SetImmortal();
	}
}

public class Immortal<T> : MonoBehaviour where T : Immortal<T>
{
    private static T m_Instance = null;
	public static T instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = GameObject.FindObjectOfType( typeof( T ) ) as T;

				if (m_Instance == null)
				{
                    if (canCreate)
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    //if (m_Instance == null)
                    //{
                        //Debug.LogError( "Immortal Intance Init ERROR - " + typeof( T ).ToString() );
                    //}
				}
                else
				    m_Instance.Init();
			}
			return m_Instance;
		}
	}

	public int InstanceID;
	public new Transform transform { get; private set; }
	public new GameObject gameObject { get; private set; }
    static bool canCreate = true;
    
	public virtual void Awake()
	{
        Init();
	}

	protected virtual void Init()
    {
        if (m_Instance == null)
        {
            transform = base.transform;
            gameObject = base.gameObject;
            InstanceID = GetInstanceID();

            m_Instance = this as T;
            DontDestroyOnLoad(base.gameObject);
        }
        else
        {
            if (m_Instance != this)
            {
                DestroyImmediate(base.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        canCreate = false;
        m_Instance = null;
    }
}


public class Rc_Global<T> : MonoBehaviour where T : Rc_Global<T>             //Scene이 넘어갈 때 사라지게 한다. 
{
    private static T m_Instance = null;
    public static T instance
    {
        get
        {
            if (m_Instance == null) //GameObject가 이미 Scene레 만들어져 있고 Component로 붙어 있다면 이쪽으로 안들어 오고, GameObject부터 만들어야 하면 이쪽으로 들어온다.
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null)
                {
                    if (canCreate)
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    //if (m_Instance == null)
                    //{
                    //Debug.LogError( "Immortal Intance Init ERROR - " + typeof( T ).ToString() );
                    //}
                }
                else
                    m_Instance.Init();
            }
            return m_Instance;
        }
    }

    public int InstanceID;
    static bool canCreate = true;

    public virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (m_Instance == null)
        {
            InstanceID = GetInstanceID();

            m_Instance = this as T;
        }
        else
        {
            if (m_Instance != this)
            {
                DestroyImmediate(base.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        canCreate = false;
        m_Instance = null;
    }
}
