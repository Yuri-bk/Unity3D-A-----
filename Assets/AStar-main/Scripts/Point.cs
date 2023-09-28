using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AStar_Yuri_bk0717
{
    /// <summary>
    /// AStar�� ��ͼ�ϸ� ���㡱 �����ԣ������ͼ������
    /// ���ص�����Prefab��
    /// </summary> 
    public class Point : MonoBehaviour
    {
        public Point Parent { get; set; }   //���׽ڵ㣬���ں��浹���γɡ�·����

        /// <summary>
        /// G �� ��㵽����һ��ָ�����ӵ��ƶ��ķ�
        /// H �� ָ�����ӣ����յ��Ԥ�ƺķ�
        /// F = G + H; �ܺķ�
        /// </summary>
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        /// <summary>
        /// ��������ڵ�ͼ�ϵ�����ֵ��������X , Z����Ϊ��άͼ
        /// �Ǿֲ�����ֵ
        /// </summary>
        public int X { get; set; }
        public int Z { get; set; }

        //����㣬�ǲ����ϰ��� -- ���� 
        public bool IsWall { get; set; }

        //�¼����Ƿ񱻵��
        public Action OnClick;  

        //�õ㣬�ϵ���Ϸ����
        //public GameObject pointgameObject;
        //�� �㣬����ά�ռ��ϵ�λ��
        public Vector3 p_position;

        private void Start()
        {
            p_position = this.gameObject.transform.position;
            IsWall = false;
        }
        /*/// <summary>
        /// ���캯��
        /// �贫�������parent��X��Z��gameObject��position
        /// parent��gameObject��position����Ĭ��ֵ
        /// FGHͨ������õ�������Ҫ������
        /// iswall,��Ĭ��Ϊfalse
        /// </summary>
        public Point(int x,int z,GameObject go,Point parent = null)
        {
            this.X = x;
            this.Z = z;
            //this.gameObject = go;
            this.Parent = parent;
            this.p_position = go.transform.position;
            IsWall = false;            
        }*/

        /// <summary>
        /// ���¸��ڵ㡢����G �� F
        /// </summary>
        /// <param name="parent">Ҫ��ɸ��ڵ�ĵ�</param>
        /// <param name="g">���¸��ڵ��g</param>
        public void UpdateParent(Point parent,float g)
        {
            this.Parent = parent;
            this.G = g;
            //�������F
            F = G + H;
        }

        /// <summary>
        /// �����Points������ɫ
        /// </summary>
        /// <param name="color">Ҫ���ĵ���ɫ</param>
        public void ChangeColor(Color color)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = color;
        }

        //ί�а�ģ�壬�������¼�������ʵ����RunTest
        public void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}
