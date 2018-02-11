using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class MyInputData : BTreeTemplateData
    {
        //坐标
        public int x;
        public int y;
        //目标点
        public int tar_x;
        public int tar_y;
        //朝向
        public int dir_x;
        public int dir_y;
    }
    public class MyOutputData : BTreeTemplateData
    {
        //坐标
        public int x;
        public int y;
        //目标点
        public int tar_x;
        public int tar_y;
        //朝向
        public int dir_x;
        public int dir_y;
    }
}
