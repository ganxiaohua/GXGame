using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameFrame;
using GXGame;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    async void Start()
    {
        await GXGameFrame.Instance.Start();
        SceneFactory.ChangePlayerScene<BattleGroudScene>();
    }

    // Update is called once per frame
    private int Entity1id;
    private GameObjectObjectBase GOB;
    void Update()
    {
        GXGameFrame.Instance.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Entity1 entity1 = SceneFactory.GetPlayerScene().AddChild<Entity1, int>(5);
            Entity1id = entity1.ID;
            CreateComponent createComponent = SceneFactory.GetPlayerScene().AddComponent<CreateComponent, int>(1);
            for (int i = 0; i < 2; i++)
            {
                createComponent.AddChild<Entity1, int>(5);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AsynTest();
            EventManager.Instance.Send<EventTest, int, int>(1, 2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SceneFactory.GetPlayerScene().RemoveChild(Entity1id);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneFactory.GetPlayerScene().AddComponent<Bttleground>();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneFactory.GetPlayerScene().RemoveComponent<Bttleground>();
        }
    }

    public async UniTask AsynTest()
    {
        await EventManager.Instance.SendAsyn<EventTestAsyn, string>("你好");
        Debug.Log("時間等待結束");
    }
}