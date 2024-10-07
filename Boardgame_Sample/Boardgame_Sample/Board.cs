namespace Boardgame_Sample
{
    internal struct Coord
    {
        public int x, y;

        public Coord(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public static bool operator ==(Coord a, Coord b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Coord a, Coord b) => a.x != b.x || a.y != b.y;
    }

    internal class Node
    {
        public Node(int x, int y)
        {
            coord = new Coord(x, y);
        }
        public Coord coord;
        public Unit? unit;
    }

    internal class Unit
    {
        public int layer;
        public int hp;
        public int atk;
        public int speed;
        public int attackRange;
    }

    internal class Board
    {
        public Board(Coord maxCoord)
        {
            nodes = new Node[maxCoord.x + 1, maxCoord.y + 1];

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j= 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }
        }

        public Node[,] nodes;
        public void SpawnUnit(int x, int y, Unit unit)
        {
            nodes[x, y].unit = unit;
        }

        public async Task StartSimulationAsync(int tickDelayMS = 0)
        {
            bool isFinished = false;

            while (isFinished == false)
            {
                DisplayMap();
                isFinished = SimulationTick();
                await Task.Delay(tickDelayMS);
            }
        }

        public bool SimulationTick()
        {
            bool isFinished = false;
            List<(Coord, Coord)> movementBuffer = new List<(Coord, Coord)>();

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Unit unit = nodes[i, j].unit;

                    if (unit != null)
                    {
                        Coord currentCoord = new Coord(i, j);

                        // 공격 범위 내에서 다른 레이어의 유닛을 찾음
                        Node targetNode = FindNodeInRange(currentCoord, unit.attackRange, node => node.unit != null && node.unit.layer != unit.layer);

                        if (targetNode != null)
                        {
                            // 공격 대상이 있으면 공격 수행
                            targetNode.unit.hp -= unit.atk;
                            if (targetNode.unit.hp <= 0)
                            {
                                targetNode.unit = null; // 유닛이 사망하면 제거
                            }
                        }
                        else
                        {
                            // todo -> Find nearest different layer unit and chase 1 index meter to the unit.
                            Node nearestTargetNode = FindNearestTarget(currentCoord, unit.layer);

                            if (nearestTargetNode != null)
                            {
                                Coord targetCoord = nearestTargetNode.coord;

                                // 한 칸씩 이동 (x와 y 중 먼저 이동 가능한 방향으로 이동)
                                Coord newCoord = GetNextStepTowards(currentCoord, targetCoord);
                                if (newCoord != currentCoord && nodes[newCoord.x, newCoord.y].unit == null)
                                {
                                    // 유닛 이동
                                    nodes[newCoord.x, newCoord.y].unit = unit;
                                    nodes[i, j].unit = null; // 기존 위치에서 유닛 제거
                                    movementBuffer.Add((newCoord, new Coord(i, j)));
                                }
                            }
                        }
                    }
                }
            }
            if (IsBattleOver())
            {
                isFinished = true;
            }

            return isFinished;
        }

        public Node FindNodeInRange(Coord coord, int range, Func<Node, bool> condition)
        {
            // 맨해튼 거리 내의 좌표만 탐색
            for (int dx = -range; dx <= range; dx++)
            {
                int remainingRange = range - Math.Abs(dx);

                for (int dy = -remainingRange; dy <= remainingRange; dy++)
                {
                    int x = coord.x + dx;
                    int y = coord.y + dy;

                    // 경계 체크 (보드의 범위를 벗어났는지 확인)
                    if (x < 0 || x >= nodes.GetLength(0) || y < 0 || y >= nodes.GetLength(1))
                        continue;

                    Node node = nodes[x, y];

                    // 주어진 조건에 맞는 노드가 있는지 확인
                    if (condition.Invoke(node))
                    {
                        return node;
                    }
                }
            }

            // 조건을 만족하는 노드가 없는 경우 null을 반환
            return null;
        }
        public Node FindNearestTarget(Coord coord, int layer)
        {
            Node nearestNode = null;
            int nearestDistance = int.MaxValue;

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Node node = nodes[i, j];

                    if (node.unit != null && node.unit.layer != layer)
                    {
                        int distance = Math.Abs(coord.x - i) + Math.Abs(coord.y - j); // 맨해튼 거리 계산

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestNode = node;
                        }
                    }
                }
            }

            return nearestNode;
        }

        // 타겟 방향으로 한 칸 이동하는 함수
        public Coord GetNextStepTowards(Coord current, Coord target)
        {
            int dx = target.x - current.x;
            int dy = target.y - current.y;

            // x 방향 또는 y 방향으로 한 칸 이동
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                return new Coord(current.x + Math.Sign(dx), current.y);
            }
            else if (Math.Abs(dy) > 0)
            {
                return new Coord(current.x, current.y + Math.Sign(dy));
            }

            // 현재 위치가 타겟 위치와 같다면 이동하지 않음
            return current;
        }

        // 특정 레이어의 유닛이 존재하는지 확인하는 함수
        public bool IsBattleOver()
        {
            bool[] layersPresent = new bool[2]; // 레이어는 0~1라고 가정

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Node node = nodes[i, j];
                    if (node.unit != null)
                    {
                        layersPresent[node.unit.layer] = true; // 해당 레이어의 유닛이 존재함
                    }
                }
            }

            // 하나 이상의 레이어에 유닛이 남아있는지 확인
            bool multipleLayersPresent = false;
            bool firstLayerFound = false;

            for (int i = 0; i < layersPresent.Length; i++)
            {
                if (layersPresent[i])
                {
                    if (!firstLayerFound)
                    {
                        firstLayerFound = true;
                    }
                    else
                    {
                        multipleLayersPresent = true; // 두 개 이상의 레이어가 있으면 전투 계속
                        break;
                    }
                }
            }

            // 두 개 이상의 레이어가 없으면 전투가 끝남
            return !multipleLayersPresent;
        }

        public void DisplayMap()
        {
            Console.WriteLine("--------------Render---------------");
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (nodes[i, j].unit != null)
                    {
                        if (nodes[i, j].unit.layer == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("♡ ");
                        }
                        else if (nodes[i, j].unit.layer == 1)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("♣ ");
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("□ ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
