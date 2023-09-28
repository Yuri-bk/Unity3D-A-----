using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AStar_Yuri_bk0717
{
    /// <summary>
    /// �˽ű�����Ҫ���������㷨ʵ��չʾ�ģ��߼���
    /// </summary>
    public class RunTest : MonoBehaviour
    {
        //��ȡ��ͼ�����ű�
        public MapMeshCreate mapMeshCreate;        

        //���������ϰ���Ƶ��
        [Header("�ϰ�����Ƶ�ʣ�")]
        [Range(0, 1)]
        public float probability;

        bool isStartFind = false;   //�Ƿ�ʼѰ·

        //��¼���������
        public int clickNum = 0;

        //Ѱ·��ʼ�㣬�յ��Point���󣬺ô���Ѱ·�㷨
        Point start;
        Point end;

        /// <summary>
        /// ���¿ո������ɣ����ã���ͼ������S����ʼѰ·�㷨
        /// </summary>
        void Update()
        {
            //���¿ո�
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                Run();
                //isCreateMap = true;
                //���¿ո�����ռ���
                clickNum = 0;
            }

            //����S�� -- ��ʼѰ·�㷨��˵����ʱ
            if (isStartFind == true && Input.GetKeyDown(KeyCode.S))
            {
                //���£���ʼѰ·�����clickNumҲ��0���ô����󣬽�������
                clickNum = 0;
                //Debug.Log("here45-run");

                //1.ʵ����װ�㷨�� -- AStarWrapper
                AStarWrapper aStarWrapper = new AStarWrapper();
                //2. �����㷨��ʼ��
                aStarWrapper.Init(mapMeshCreate, start, end);
                //3. �����߳�
                StartCoroutine(aStarWrapper.OnStart());

                isStartFind = false;
            }
        }

        /// <summary>
        /// ���У�����mapMeshCreate�����ɵ�ͼ
        /// </summary>
        private void Run()
        {
            //����ί���¼���
            mapMeshCreate.PointEvent = PointEvent;
            //���ɵ�ͼ
            mapMeshCreate.CreateMap();
        }

        /// <summary>
        /// ����mapMeshCreate��pointEventִ�з�����ͨ��ί�д���
        /// </summary>
        /// <param name="go"></param>
        /// <param name="row"></param>
        /// <param name="row"></param>
        private void PointEvent(GameObject go,int row,int column)
        {
            //1.����Ҫ��������ϰ��ˣ���ȡ�������point���
            Point point = go.GetComponent<Point>();
            //2. �����ϰ�����Ƶ�ʣ���ȷ����ǰ��������Ƿ����ɫ
            float f = Random.Range(0, 1.0f);
            Color color = f <= probability ? Color.red : Color.white;
            //3. ��ʼ������
            point.ChangeColor(color);
            point.IsWall = f <= probability;
            point.X = row;
            point.Z = column;

            //������������¼���ģ�����¼�
            point.OnClick = () =>
            {
                if (point.IsWall)
                {
                    //����������ϰ���ֱ�ӷ��ز�������
                    return;
                }

                //���������1��
                clickNum++;

                Debug.Log(point.X);
                Debug.Log("�Ƿ�Ϊ�ϰ�:" + point.IsWall.ToString());
                //���һ��˵������㣬�ڶ���˵�����յ㡣
                switch (clickNum)
                {
                    case 1:
                        start = point;
                        point.ChangeColor(Color.yellow);
                        break;
                    case 2:
                        end = point;
                        point.ChangeColor(Color.yellow);
                        isStartFind = true; //�����������Ϳ��Կ�ʼѰ·
                        break;
                    default:
                        isStartFind = true; //�����������Ϳ��Կ�ʼѰ·
                        break;
                }
            };
        }
    }
}