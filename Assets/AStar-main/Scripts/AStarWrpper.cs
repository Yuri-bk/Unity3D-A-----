using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar_Yuri_bk0717
{
    /// <summary>
    /// �����ã� ��װ A���㷨����
    /// ʵ��ԭ��
    /// 1��������һ��һ����ߵĵ�ͼ ������� Point ��ĵ�ͼ������ Point ���� IsWall ���ԣ�
    /// 2���趨��ʼ�㣬��Ŀ���
    /// 3������ FindPath ��ʼѰ�ҽ϶�·�����ҵ�����true������ false
    /// 4��Ϊ true �Ϳ���ͨ�� Ŀ���ĸ��׵�ĸ��׵�ĸ��׵㣬ֱ�����׵�Ϊ��ʼ�㣬
    ///    ��Щ�㼯�ϼ���·��
    /// 5��FindPath Ѱ��ԭ��
    /// 1�����б����б��ʼ��
    /// 2����ӿ�ʼ�㵽���б�Ȼ������Χ�㼯�ϣ������ְѿ�ʼ��ӿ��б����Ƴ���
    ///    ����ӵ����б�
    /// 3���ж���Щ��Χ�㼯���Ƿ��Ѿ��ڿ��б��У������������Щ���F �� ���׵㣬
    ///    ����ӵ����б��������¼���Gֵ��G��С�����GF �͸��׵�
    /// 4������Χ�㼯�����ҵ� F ��С�ĵ㣬Ȼ������Χ�㼯�ϣ������ְ��ҵ� F ��С�ĵ�
    ///    �ӿ��б����Ƴ�������ӵ����б�
    /// 5������ִ�е� 3�� ����
    /// 6��ֱ��Ŀ��㱻��ӵ����б��У���·���ҵ�
    /// 7������ֱ�����б���û�������ݣ���˵��û�к���·��
    /// </summary>
    class AStarWrapper
    {
        /* ���Դ��㷨 ��Ҫ���¼��������� 
         *���ܣ�FindPath��ȡ�ý϶�·����
         * �ܷ����漰�ĺ�����
         * PointFilter���ѹر��б�㣬����Χ���Ϲ���
           GetSurrondPoint����ȡ��ǰ��СF��Χ��
           FindMinOfPoint�����б�����F���ҳ���С��F��
           CalG��CalF������G��F��H�Ƚϼ򵥣�����װ��*/

        public MapMeshCreate mapMeshCreate;
        public Point Start;
        public Point End;
        public Point[,] map;
        public int mapWidth;
        public int mapHeight;
        //��һ��ջ�����·���㣬��ʱ��ֱ�ӵ���ջͬʱȾɫ��
        public Stack<Point> resultPoints;
        //���б���Ҫ���ǵĵ㣻���б����迼�ǵĵ㼯�ϣ�
        public List<Point> openList = new List<Point>();
        public List<Point> closedList = new List<Point>();

        //���ҵ���·������ʱ��openlist���ذѷ���Ⱦ���̵�

        /// <summary>
        /// �����㷨����FindPath��������������ű������ĳ�ʼ��
        /// </summary>
        public void Init(MapMeshCreate mapMesh,Point sta,Point end)
        {
            this.mapMeshCreate = mapMesh;
            map = mapMeshCreate.m_Popo;
            this.Start = sta;
            this.End = end;
            resultPoints = new Stack<Point>();
            openList = new List<Point>();
            closedList = new List<Point>();

            mapWidth = mapMesh.meshRange.horizontal;
            mapHeight = mapMesh.meshRange.vertical;
        }

        /// <summary>
        /// Ϊ���ڱ�������Ϊ��ʹ������Ѱ·���̵Ĳ�����ӻ���
        /// ʹ��һ��Э�������Ѱ·���̵ķ������ã���������ÿһ�����һ���Ѱ·��
        /// ����ͨ��Э������ʱִ����һ��ѭ��
        /// </summary>
        /// <returns></returns>
        public IEnumerator OnStart()
        {
            bool isHaveResult = FindPath(Start, End, map, mapWidth, mapHeight);
            if(isHaveResult == true)
            {
                yield return new WaitForSeconds(1);
                Traverse();
            }
            else
            {
                yield return new WaitForSeconds(1);
                Debug.Log("�Ҳ������·����sorry~");
            }
        }

        /// <summary>
        /// �ҵ���·���󣬸��ݡ��Ϳ���ͨ�� Ŀ���ĸ��׵�ĸ��׵�ĸ��׵㣬
        ///                 ֱ�����׵�Ϊ��ʼ�㣬��Щ�㼯�ϼ���·����
        /// ������Щ�㻻��ɫ
        /// </summary>
        public void Traverse()
        {            
            if(openList.Count == 0)
            {
                //˵��û��·��
                return;
            }

            //��Ϊ�˺�����ǰ�������ǣ�FindPath����true���ŵ�����������
            //1.��Ŀ��㸳��po
            Point po = openList[openList.IndexOf(End)];
            while(po.Parent != Start)
            {
                //2.po = po�ĸ��ڵ�
                po = po.Parent;
                //3.��po����ɫ����
                po.ChangeColor(Color.green);                
                //4.ѭ��������ֱ��·����ʼ�Ŀ�ʼ�㣬��ʼ��ĸ��ڵ�Ϊnull��
            }
        }

        /// <summary>
        /// �������ã�ȡ����㵽�յ�Ľ϶�·��--���㷨����Ŀ��
        /// </summary>
        /// <param name="start">Point���</param>
        /// <param name="end">Point�յ�</param>
        /// <param name="map">Point�����ͼ</param>
        /// <param name="mapWidth">��ͼ��</param>
        /// <param name="mapHeight">��ͼ��</param>
        /// <returns>true�������ҵ�·����
        /// false���Ҳ���·��</returns>
        public bool FindPath(Point start,Point end, Point[,] map, int mapWidth,int mapHeight)
        {
            //�㷨��ʼ��1.�ѿ�ʼ����ӽ����б�
            openList.Add(start);
            //2. �Կ��б�ĵ㣬ֻҪ���б��е㣬�㷨��ͣ��
            //ֱ��û�е㣬�򷵻�false��˵���Ҳ���·��
            while(openList.Count > 0)
            {
                //3.Ѱ����Χ�� F��С�ĵ㣬���ú���--FindMinOfPoint
                Point point = FindMinOfPoint(openList);

                //4.�ҵ���СF�ĵ������������·���ĵ㣬�����Ƴ����б��Ž����б�
                openList.Remove(point);
                closedList.Add(point);
                //resultPoints.Push(point);

                //5.��СF ת���ɵ�ǰ�㣬������������������Χ�㼯��
                List<Point> surrendedList = GetSurrondPoint(point, map, mapWidth, mapHeight);

                //6.����Χ�㼯�ϣ�������һ�ι��ˣ���Ϊ��������б�ĵ㣻
                PointFilter(surrendedList, closedList);

                //7. ����������������õ���Χ�㼯�ϣ�
                foreach(Point item in surrendedList)
                {
                    //8.1�� ������ڿ��б��У�˵���˵�Ҳ��֮ǰ����Χ���г��ֹ�
                    //�����Gֵ����һ�����ᣬ��һ��С���أ��ȽϺ�Ҫ���µľ͸���
                    if(openList.IndexOf(item) > -1)
                    {
                        //9.1��  ����Gֵ ��Ϊ�ڿ��б��򸸽ڵ�Ϊ��СF
                        float nowG = CalG(item, point);
                        //9.2: Gֵ��С�Ļ���
                        if(nowG < item.G)
                        {
                            //����СF������ǰ��ĸ��ڵ㣬�Ҹ��µ�ǰ���G
                            item.UpdateParent(point, nowG);                            
                        }
                    }
                    //8.2�� ���㲻���ڿ��б��У�˵����һ�������õ꣬����СF�����ڵ��
                    else
                    {
                        //9.3�� ��СF����ǰ��ĸ��ڵ�
                        item.Parent = point;
                        //9.4�� ������º��FGH
                        CalF(item, end);
                        //9.5�� ��ӵ����б�
                        openList.Add(item);                        
                    }
                }

                //10.ѭ����������Χ�������ϣ�Ҳѡ���˵�ǰ��ĺ�·����ʱ
                //����������һЩ��ѭ���Ľ���������
                //��end ������openList�У�˵�������ҵ���һ��·��������true                               
                if(openList.IndexOf(end) > -1)
                {
                    //��ʱ��end���Ѿ�������openlist����
                    return true;
                }
            }

            //��ѭ��������openList�ﶼ�������ֹ�end�յ㣬˵���Ҳ���·����������false
            return false;
        }

        /// <summary>
        /// ���б����е㣬�ҳ���С��F�ĵ�
        /// </summary>
        /// <param name="openList">���б�</param>
        /// <returns>���������СF�ĵ�</returns>
        private Point FindMinOfPoint(List<Point> openList)
        {
            //�������б��ҵ�������С��f
            float minVal = float.MaxValue;  //�������������Ƚ�
            Point temp = null; //��ʱ���� 

            foreach(Point p in openList)
            {
                //ÿ�ΰѽ�С��ֵ����minVal�������Ƚϡ�
                if(p.F < minVal)
                {
                    temp = p;
                    minVal = p.F;
                }
            }

            //ѭ������������ʱ�㣬������С�ĵ�
            return temp;
        }

        /// <summary>
        /// ��ȡ��ǰ��СF��ģ���Χ�㼯��
        /// </summary>
        /// <param name="point">��ǰ��СF��</param>
        /// <param name="map">��ͼ</param>
        /// <param name="mapWidth">��ͼ��</param>
        /// <param name="mapHeight">��ͼ��</param>
        /// <returns></returns>
        private List<Point> GetSurrondPoint(Point point,Point[,] map,int mapWidth, int mapHeight)
        {
            //1.һ��һ������Χ�У��������ң����������������� 8���㣬�ȶ������
            Point up = null, down = null, left = null, right = null,
                lu = null, ld = null, ru = null, rd = null;

            //2.����������ȡ�㿩������еĻ���

            //�ж������ϵ� �� ����Zֵ��С�ڵ�ͼ-1.˵��������ˣ����ϵ�
            //�ϵ��ж�,ȡ�ϵ㣨����еĻ���
            if (point.Z < mapHeight - 1)
            {
                up = map[point.X, point.Z + 1];
            }
            //�µ��ж�,ȡ�µ㣨����еĻ���
            if (point.Z > 0)
            {
                down = map[point.X, point.Z - 1];
            }
            //����ж�,ȡ��㣨����еĻ���
            if(point.X > 0)
            {
                left = map[point.X - 1, point.Z];
            }
            //����ж�,ȡ�ҵ㣨����еĻ���
            if (point.X < mapWidth - 1)
            {
                right = map[point.X + 1, point.Z];
            }

            //���������������� �� �жϣ�
            //���ϵ� �� ��㣬��˵��������
            if(up != null && left != null)
            {
                lu = map[point.X - 1, point.Z + 1];
            }
            //���µ� �� ��㣬��˵��������
            if (down != null && left != null)
            {
                ld = map[point.X - 1, point.Z - 1];
            }
            //���ϵ� �� �ҵ㣬��˵��������
            if (up != null && right != null)
            {
                ru = map[point.X + 1, point.Z + 1];
            }
            //���µ� �� �ҵ㣬��˵��������
            if (down != null && right != null)
            {
                rd = map[point.X + 1, point.Z - 1];
            }

            //3.��һ���б��Ѻϸ�ı�Ž���Χ���б���
            //������ �㲻Ϊ�գ��Ҳ�Ϊ�ϰ����ɷŽ��б���
            List<Point> List = new List<Point>();

            if(up != null && up.IsWall == false)
            {
                List.Add(up);   //��
            }
            if (down != null && down.IsWall == false)
            {
                List.Add(down); //��
            }
            if (left != null && left.IsWall == false)
            {
                List.Add(left);   //��
            }
            if (right != null && right.IsWall == false)
            {
                List.Add(right);   //��
            }

            //���������������� �����⣺�磺���ϵ㲻Ϊ�գ������� �����϶���Ϊ�ϰ�����
            if(lu != null && lu.IsWall == false && left.IsWall == false && up.IsWall == false)
            {
                List.Add(lu);   //����
            }
            if (ld != null && ld.IsWall == false && left.IsWall == false && down.IsWall == false)
            {
                List.Add(ld);   //����
            }
            if (ru != null && ru.IsWall == false && right.IsWall == false && up.IsWall == false)
            {
                List.Add(ru);   //����
            }
            if (rd != null && rd.IsWall == false && right.IsWall == false && down.IsWall == false)
            {
                List.Add(rd);   //����
            }

            //4.��������ˣ�ֱ�ӷ����б�
            return List;
        }
    
        /// <summary>
        /// ����Χ�㼯����һ�����ˣ�ȥ�����б�ĵ�
        /// </summary>
        /// <param name="surrendList">��Χ�㼯��</param>
        /// <param name="closedList">���б�</param>
        private void PointFilter(List<Point> surrendList , List<Point> closedList)
        {
            //�������б���������Χ����Ƴ���Χ���б�
            foreach(Point item in closedList)
            {
                if(surrendList.IndexOf(item) > -1)
                {
                    //���ڣ����Ƴ�
                    surrendList.Remove(item);
                }
                //����������������ѭ��
            }
        }
    
        /// <summary>
        /// ���㵱ǰ���Gֵ
        /// </summary>
        /// <param name="now">��ǰ��</param>
        /// <param name="parent">���ڵ�</param>
        /// <returns></returns>
        private float CalG(Point now,Point parent) 
        {
            //��now��parent��֮��ľ��룬+ ���ڵ��G ��= ��ǰ��Gֵ
            return (Vector2.Distance(new Vector2(now.X, now.Z)
                , new Vector2(parent.X, parent.Z)) + parent.G);
        }

        /// <summary>
        /// ���㵱ǰ��� F ֵ
        /// </summary>
        /// <param name="now">��ǰ��</param>
        /// <param name="end">�յ�</param>        
        private void CalF(Point now, Point end)
        {
            //1.�ȼ��� ��ǰ��� H ֵ�����㷨���յ��X��Z�ֱ��뵱ǰ���X��Z��������
            //ȡ���Լ����ľ���ֵ����Ӿ���H
            float h = Mathf.Abs(end.X - now.X) + Mathf.Abs(end.Z - now.Z);
            //2.Gֵ��Ĭ��Ϊ0
            float g = 0;
            
            //3.��Ϊ��ǰ�㣬��һ���и��ڵ㣬�������жϣ�
            if(now.Parent == null)
            {
                //û�и��ڵ㣬��G����0��
                g = 0;
            }
            else
            {
                //�и��ڵ㣬��G��������һ��
                g = (Vector2.Distance(new Vector2(now.X, now.Z)
                , new Vector2(now.Parent.X, now.Parent.Z)) + now.Parent.G);
            }

            //G ��H ���õ��ˣ����µ�ǰ��� g f h ֵ
            float f = g + h;

            now.F = f;
            now.G = g;
            now.H = h;
        }
    }
}
