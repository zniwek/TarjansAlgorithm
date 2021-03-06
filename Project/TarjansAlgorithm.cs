﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public partial class Graph
    {
        //Tarjan's algorithm output - list of bridges
        //integer arrays in list always have 2 elements - 2 vertices of an edge
        //used in main window
        public List<int[]> Bridges { get; private set; }

        int iterator_one = 1;

        int[] dfs_numbers;
        int?[] parents_array;
        List<int>[] spanning_tree_2;
        int[] low_array; //array for low parameter for every vertex
        bool[] visited;

        public void Tarjan_Bridges()
        {
            Bridges = new List<int[]>();
            visited = new bool[AdjacencyList.Count];
            low_array = new int[AdjacencyList.Count];
            spanning_tree_2 = new List<int>[AdjacencyList.Count];
            dfs_numbers = new int[AdjacencyList.Count];
            Set_Spanning_Tree();
            Set_DFS_Array();
            Set_Parents_Array();
            set_Tarjan_Bridges();
            
            iterator_one = 1;
        }
         void set_Tarjan_Bridges()
        {
            int first_vertex = 0;
            while (isVisited() == false)
            {
                Tarjan_Bridges( first_vertex++);

            }
            Clearing_Bool_Table();

        }
        void Tarjan_Bridges(int First_Vertex)
        {

            if (visited[First_Vertex] == true)
            {
                return;
            }
            visited[First_Vertex] = true;


            List<int> vList = AdjacencyList[First_Vertex];
            foreach (var n in vList)
            {
                if (!visited[n]) //unvisited neighboor
                {
                    Tarjan_Bridges(n);
                }

            } //end of branch

            int low_tmp = dfs_numbers[First_Vertex]; //first initialization of low parameter
            //checking for sons' low parameters
            for (int i = 0; i < spanning_tree_2[First_Vertex].Count; i++)
            {
                if (low_array[spanning_tree_2[First_Vertex][i]] != 0 && low_array[spanning_tree_2[First_Vertex][i]] < low_tmp)
                {
                    low_tmp = low_array[spanning_tree_2[First_Vertex][i]];
                }
            }
            //checking for other 'current vertex' neighbors

            for (int i = 0; i < AdjacencyList[First_Vertex].Count; i++)
            {
                bool returning_edge = true;
                if (AdjacencyList[First_Vertex][i] == parents_array[First_Vertex]) //father
                {
                    returning_edge = false;
                }
                if (AdjacencyList[First_Vertex][i] != parents_array[First_Vertex]) //not father
                {

                    for (int j = 0; j < spanning_tree_2[First_Vertex].Count; j++) //but son
                    {
                        if (AdjacencyList[First_Vertex][i] == spanning_tree_2[First_Vertex][j])
                        {
                            returning_edge = false;
                        }
                    }


                }//not father nor son
                if (returning_edge == true)
                {
                    if (dfs_numbers[AdjacencyList[First_Vertex][i]] < low_tmp)
                    {
                        low_tmp = dfs_numbers[AdjacencyList[First_Vertex][i]];
                    }
                }

            }
            low_array[First_Vertex] = low_tmp;
            if (low_array[First_Vertex] == dfs_numbers[First_Vertex] && parents_array[First_Vertex] != null)
            {

                Bridges.Add(new int[] { First_Vertex, (int)parents_array[First_Vertex] });
            }


        }
        void Set_Spanning_Tree() //dfs spanning tree
        {
            int first_vertex = 0;

            for (int i = 0; i < spanning_tree_2.Length; i++)
            {
                spanning_tree_2[i] = new List<int>();
            }
            while (isVisited() == false)
            {
                DFS_Spanning_Tree_R(first_vertex++);
            }
            Clearing_Bool_Table();
        }
         bool isVisited()
        {
            bool visited_array = true;
            for (int i = 0; i < visited.Length; i++)
            {
                if (visited[i] == false)
                {
                    visited_array = false;
                }
            }
            return visited_array;
        }
        void Set_DFS_Array() //array for dfs numbers of vertices
        {
            int first_vertex = 0;

            while (isVisited() == false)
            {
                DFS_Array_R(first_vertex++);

            }
            Clearing_Bool_Table();
        }
        void Set_Parents_Array() //array of parents for every vertex
        {
            parents_array = DFS_Parents_Array();
            Clearing_Bool_Table();
        }

        void Clearing_Bool_Table()
        {
            for (int i = 0; i < visited.Length; i++)
            {
                visited[i] = false;
            }
        }
        void DFS_Spanning_Tree_R(int First_Vertex) //altering array of lists building up DFS spanning tree, recursive version
        {
            //version where spanning tree doesnt have child-parent edges, only edges from parent to child
            if (visited[First_Vertex] == true)
            {
                return;
            }


            visited[First_Vertex] = true;

            List<int> vList = AdjacencyList[First_Vertex];
            foreach (var n in vList)
            {
                if (!visited[n])
                {
                    spanning_tree_2[First_Vertex].Add(n);
                    DFS_Spanning_Tree_R(n);
                }

            }
           
        }
        void DFS_Array_R(int First_Vertex)//array for vertices dfs numbers
        {
            if (visited[First_Vertex] == true)
            {
                return;
            }
            visited[First_Vertex] = true;
            dfs_numbers[First_Vertex] = iterator_one;
            iterator_one++;

            List<int> vList = AdjacencyList[First_Vertex];
            foreach (var n in vList)
            {
                if (!visited[n])
                    DFS_Array_R(n);
            }
            
        }

        int?[] DFS_Parents_Array() //array for vertices' parents
        {
            int?[] parents_array = new int?[spanning_tree_2.Length];
            for (int i = 0; i < spanning_tree_2.Length; i++) //parents
            {
                for (int j = 0; j < spanning_tree_2[i].Count; j++) //kids
                {
                    parents_array[spanning_tree_2[i][j]] = i;
                }
            }
            return parents_array;
        }

    }
}
