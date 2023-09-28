using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AStar_Yuri_bk0717
{
    /// <summary>
    /// ���ص��ű��ϣ����ݵ�ͼ���ԣ����ɵ�ͼ����ʾ��ͼ����
    /// </summary>
    public class MapMeshCreate : MonoBehaviour
    {
        /// <summary>
        /// ��ͼ��Χ�����������
        /// </summary>
        [Serializable]
        public class MeshRange
        {
            public int horizontal;
            public int vertical;
        }

        /// <summary>
        /// һЩ��ʾ�ڽű��ϣ���Ҫ�û���ʼ����UI
        /// </summary>
        [Header("��ͼ��Χ������")]
        public MeshRange meshRange;
        [Header("��ͼ��ʼ�㣺")]
        public Transform startPoint;
        private Vector3 startPos;
        [Header("������ͼ����ģ����ڵ�--ԭ�㣺")]
        public Transform parentTrans;
        [Header("��ͼ���ӵ�Ԥ���壺")]
        public GameObject p_Prefab;
        [Header("��ͼģ���С��")]
        public Vector2 scale;

        private void Start()
        {
            startPos = startPoint.position;
        }
        

        //��������
        private GameObject[,] m_Points;
        //ȡ������ķ�����
        public GameObject[,] points
        {
            get
            {
                return m_Points;
            }
        }
        //ȡ������Ԥ���巽���Ϲ��صĽű�Point������
        public Point[,] m_Popo;
        public Point[,] popo
        {
            get
            {
                return m_Popo;
            }
        }

        //ע��ģ���¼� -- ����������¼�
        public Action<GameObject, int, int> PointEvent;

        /// <summary>
        /// ���ڹ�������ĳ�ʼ�����ݣ������ĵ�ͼ
        /// </summary>
        public void CreateMap()
        {
            //1.�����ͼ��Χ������һ��û�У��򴴽����˵�ͼ��ֱ�ӷ���
            if(meshRange.horizontal == 0 || meshRange.vertical == 0)
            {
                return;
            }
            //2.���ԭ���ĵ�ͼ��
            ClearMap();           

            //3.��ʼ���������� -- ���ݵ�ͼ����
            m_Points = new GameObject[meshRange.horizontal, meshRange.vertical];
            m_Popo = new Point[meshRange.horizontal, meshRange.vertical];
            //4.��ʼ���ɵ�ͼ
            for(int i = 0; i < meshRange.horizontal; i++)
            {
                for(int j = 0; j < meshRange.vertical; j++)
                {
                    //����һ���㣬�Ѵ����Ķ��󷵻��£�Ϊ�����ɺ����Point
                    CreatePoint(i, j);                    
                }
            }
        }
        /// <summary>
        /// ���غ��� ���ڴ���Ŀ�������ɵ�ͼ
        /// </summary>
        /// <param name="width">��</param>
        /// <param name="height">��ͼ��</param>
        public void CreateMap(int width,int height)
        {
            //1.���ж�
            if(width == 0 || height == 0)
            {
                return;
            }

            ClearMap();
            m_Points = new GameObject[width, height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    CreatePoint(i, j);
                }
            }

        }

        /// <summary>
        /// ɾ����ͼ����ջ���
        /// </summary>
        public void ClearMap()
        {
            if (m_Points == null || m_Points.Length == 0)
            {
                return; //˵��û�е�ͼ��
            }
            //��յ�ͼ
            foreach(GameObject go in m_Points)
            {
                if(go != null)
                {
                    Destroy(go);
                }
            }
            //��ջ��� -- ����
            Array.Clear(m_Points, 0, m_Points.Length);
            //Array.Clear(m_Popo, 0, m_Popo.Length);
        }

        /// <summary>
        /// ���ݵ㣬�������ϵ�X,Z����
        /// </summary>
        /// <param name="row">X������ -- ��</param>
        /// <param name="column">Z������ -- ��</param>
        public void CreatePoint(int row,int column)
        {
            //1.��ʵ����һ���������������ȷ����Ԥ���壬���������ص�ԭ����
            GameObject go = GameObject.Instantiate(p_Prefab, parentTrans);
            
            //2.��ʼ���������꣬��������ȫ������
            //2.1 ��ʼ�� + ģ�� * x�����꣨�ֲ����꣩
            float posx = startPos.x + scale.x * row;
            float posz = startPos.z + scale.y * column;
            go.transform.position = new Vector3(posx, startPos.y, posz);

            //3.�ѷ��飬�Ž��������У�
            m_Points[row, column] = go;
            //3.1ȡ�������Ϲ��صĽű�Point,�ѽű���X,Z��һ��
            go.gameObject.GetComponent<Point>().X = row;
            go.gameObject.GetComponent<Point>().Z = column;
            //3.4 �ѷ����Ϲ��صĽű�Point���Ž�����
            m_Popo[row, column] = go.gameObject.GetComponent<Point>();
            
            //4. ���������ϵ��¼�
            PointEvent?.Invoke(go, row, column);                      
        }
    }
}

