using System;
using System.Collections.Generic;

namespace MaxPath
{
    class Program
    {
        //Tree Node Class
        internal class Node
        {
            public int Value { get; set; }
            public int Level { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public NodeType NodeType
            {
                get
                {
                    if (Value % 2 == 0)
                        return NodeType.Even;
                    else
                        return NodeType.Odd;
                }
            }

            public Node(int itemValue, int itemLevel)
            {
                Value = itemValue;
                Level = itemLevel;
                Left = Right = null;
            }
        }
        
        //Even or Odd Type
        public enum NodeType
        {
            Even,
            Odd
        }

        //Path Response
        public class FindMaxResult
        {
            public int MaxSum { get; set; }
            public string MaxPath { get; set; }
            public int LevelOfResult { get; set; }
        }

        //Tree Generator Method -> From String To Tree
        private static Node GenerateTree(Node head, ref int levelofTree)
        {
            Queue<Node> nodeQueue = new Queue<Node>();
            int currentLevel = 1;

            var splitedTriagle = Triangle.Split("\r\n");

            foreach (var level in splitedTriagle)
            {
                var splitedLevel = level.Split(" ");
                foreach (var value in splitedLevel)
                {
                    var newNode = new Node(Convert.ToInt32(value), currentLevel);

                    if (nodeQueue.Count == 0)
                        head = newNode;
                    else
                    {
                        var tempNode = nodeQueue.Peek();

                        if (tempNode.Level == currentLevel)
                            throw new Exception("Tree is no valid! One of Levels is long.");

                        if (tempNode.Left == null)
                            tempNode.Left = newNode;
                        else if (tempNode.Right == null)
                        {
                            tempNode.Right = newNode;
                            nodeQueue.Dequeue();
                            if (nodeQueue.Count != 0 && nodeQueue.Peek().Level == (currentLevel - 1))
                            {
                                //var newNode2 = new Node(Convert.ToInt32(value), currentLevel);
                                nodeQueue.Peek().Left = newNode;
                            }
                        }
                    }

                    nodeQueue.Enqueue(newNode);
                }

                if (nodeQueue.Peek().Level == (currentLevel - 1))
                    throw new Exception("Tree is no valid! One of Levels is short.");

                levelofTree = currentLevel++;
            }

            return head;
        }

        //Path Finder
        public static FindMaxResult FindMaxSum(Node head)
        {
            if (head.Left == null)
            {
                return new FindMaxResult()
                {
                    MaxSum = head.Value,
                    MaxPath = $@"{head.Value}",
                    LevelOfResult = 1
                };
            }

            FindMaxResult left = new FindMaxResult()
            {
                MaxSum = Int32.MinValue
            };
            FindMaxResult right = new FindMaxResult()
            {
                MaxSum = Int32.MinValue
            };

            if (head.NodeType != head.Left.NodeType)
                left = FindMaxSum(head.Left);

            if (head.NodeType != head.Right.NodeType)
                right = FindMaxSum(head.Right);

            FindMaxResult maxResult = null;
            if (left.MaxSum > right.MaxSum)
                maxResult = left;
            else
                maxResult = right;

            return new FindMaxResult()
            {
                MaxSum = maxResult.MaxSum + head.Value,
                MaxPath = $@"{head.Value} -> {maxResult.MaxPath}",
                LevelOfResult = maxResult.LevelOfResult + 1
            };
        }

        private const string Triangle =
            @"215
192 124
117 269 442
218 836 347 235
320 805 522 417 345
229 601 728 835 133 124
248 202 277 433 207 263 257
359 464 504 528 516 716 871 182
461 441 426 656 863 560 380 171 923
381 348 573 533 448 632 387 176 975 449
223 711 445 645 245 543 931 532 937 541 444
330 131 333 928 376 733 017 778 839 168 197 197
131 171 522 137 217 224 291 413 528 520 227 229 928
223 626 034 683 839 052 627 310 713 999 629 817 410 121
924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

        static void Main(string[] args)
        {
            Node head = null;
            int levelofTree = 1;

            head = GenerateTree(head, ref levelofTree);

            var result = FindMaxSum(head);
            if(result.LevelOfResult != levelofTree)
                Console.WriteLine("Im Sorry :( But You didn't reach to the bottom of the pyramid. The Deepest Path : " + result.MaxPath);

            Console.WriteLine($@"Max Sum: {result.MaxSum}");
            Console.WriteLine($@"Max Path: {result.MaxPath}");
        }
    }
}
