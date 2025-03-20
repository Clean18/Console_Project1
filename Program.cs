using System;

namespace _250319_Project
{
    class Program
    {
        struct Position
        {
            public int x, y;
        }

        public static bool isClear = false;     // 클리어 유무
        public static int currentMap;       // 현재 맵 번호
        public static ConsoleKey playerFront;   // 플레이어의 마지막 방향
        public static char[,] map0;
        public static char[,] map1;
        public static char[,] map2;
        public static char[,] map3;
        public static int currentCoin; // 현재 코인 개수
        public static int totalCount;  // 전체 코인 개수
        public static int downTimer;   // 시간 체크용
        public static int upTimer;
        public static int startTime;   // 시간 제한용 콘솔 출력

        static void Main()
        {
            // 키입력 처리
            // Console.KeyAvailable

            // 컴퓨터 로컬 시간
            // Environment.TickCount

            // 기초 문법을 활용해서 콘솔 환경의 텍스트 게임 제작
            // 2차원 배열 + Console.ReadKey 를 사용해서 이동 구현하기

            // Console.Clear로 지우던가 새로 덧그리던가 Render구현

            // 2차원 배열에 들은 요소에 따라 이동 체크하기

            // 게임적인 메커니즘을 추가

            // 1. 동전찾기
            // 2. 맵 4개 연결하기
            // 3. 플레이어가 바라보는 방향에 특정 엔티티가 있을 시 스페이스바를 누르면 한칸 밀거나 부숨 = 엔티티 위치에 동전있음
            // 4. 다 모으면 클리어

            Position playerPos = new Position(); // 플레이어 포지션 초기화

            Start(out playerPos); // 초기화

            StartTitle(); // 시작화면

            PrintGameText();
            TimeText();

            while (isClear == false)
            {
                char[,] map = GetMap();
                Render(ref playerPos, ref map);
                if (Console.KeyAvailable) // 키입력 했을 때
                {
                    ConsoleKey key = Input();
                    Update(key, ref playerPos, ref map); // Update를 여기에 두면 계속 그려서 그런가 깜빡이네
                }

                if (!Console.KeyAvailable) // 키입력 안했을 때
                {
                    if (Environment.TickCount - upTimer >= 1000)
                    {
                        upTimer = Environment.TickCount;

                        startTime--;
                        if (startTime <= 0)
                        {
                            // 실패시
                            break;
                        }
                        TimeText();
                    }

                    //// 시간이 줄어들게
                    //startTime = (downTimer - Environment.TickCount) / 1000;
                    //TimeText();
                    //if (startTime <= 0)
                    //{
                    //    // 실패시
                    //    break;
                    //}
                }
            }
            End(isClear);
        }

        static void Start(out Position playerPos) // Init
        {
            Console.CursorVisible = false;

            // 플레이어 위치 초기화
            // 설정할 플레이어의 위치는 고정
            playerPos.x = 1;
            playerPos.y = 1;

            isClear = false;
            currentMap = 0;  // 현재 맵 번호

            //startTime = 15;  // n초 제한 and 콘솔 출력용 타이머
            // 15초 하드
            // 20초 노말
            // 30초 이지

            // 맵 초기화
            map0 = MapInit(0);
            map1 = MapInit(1);
            map2 = MapInit(2);
            map3 = MapInit(3);

            // CurrentCoin, TotlaCoin 초기화
            SetTotalCoinCount();
        }

        static void StartTitle() // 시작 화면
        {
            Console.Clear();
            //Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ"); // 16개
            //Console.WriteLine("ㅁ                            ㅁ");
            //Console.WriteLine("ㅁ                            ㅁ");
            //Console.WriteLine("ㅁ         동전  찾기         ㅁ");
            //Console.WriteLine("ㅁ                            ㅁ");
            //Console.WriteLine("ㅁ   아무 키나 입력해주세요   ㅁ");
            //Console.WriteLine("ㅁ                            ㅁ");
            //Console.WriteLine("ㅁ                            ㅁ");
            //Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ");

            //Console.ReadKey(false);

            int easyValue = 1;
            string[] easyString = new string[2];
            easyString[0] = "ㅁ            이지            ㅁ";
            easyString[1] = "ㅁ         >  이지            ㅁ";

            int normalValue = 0;
            string[] normalString = new string[2];
            normalString[0] = "ㅁ            노말            ㅁ";
            normalString[1] = "ㅁ         >  노말            ㅁ";

            int hardValue = 0;
            string[] hardString = new string[2];
            hardString[0] = "ㅁ            하드            ㅁ";
            hardString[1] = "ㅁ         >  하드            ㅁ";

            bool isSelect = false;
            while (!isSelect)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ"); // 16개
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ         동전  찾기         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ      방향키 : 위 아래      ㅁ");
                Console.WriteLine("ㅁ        엔터 : 선택         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine(easyString[easyValue]);
                Console.WriteLine(normalString[normalValue]);
                Console.WriteLine(hardString[hardValue]);
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ");

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.DownArrow)
                    {
                        if (easyValue == 1)
                        {
                            easyValue = 0;
                            normalValue = 1;
                        }
                        else if (normalValue == 1)
                        {
                            normalValue = 0;
                            hardValue = 1;
                        }
                        else if (hardValue == 1)
                        {
                            hardValue = 0;
                            easyValue = 1;
                        }
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        if (easyValue == 1)
                        {
                            easyValue = 0;
                            hardValue = 1;
                        }
                        else if (normalValue == 1)
                        {
                            normalValue = 0;
                            easyValue = 1;
                        }
                        else if (hardValue == 1)
                        {
                            hardValue = 0;
                            normalValue = 1;
                        }
                    }

                    if (key == ConsoleKey.Enter)
                    {
                        if (easyValue == 1)
                        {
                            startTime = 30;
                        }
                        else if (normalValue == 1)
                        {
                            startTime = 20;
                        }
                        else if (hardValue == 1)
                        {
                            startTime = 15;
                        }

                        upTimer = Environment.TickCount;
                        downTimer = (startTime * 1000) + Environment.TickCount; // 시간 지정
                        Console.Clear();
                        isSelect = true;
                    }
                }
            }
        }

        static char[,] MapInit(int currentMap) // 맵 // 수정가능
        {
            // 맵 크기     // 맵 모양      // 맵 위치
            // x10 y7      // 0 = 「       // 0 1
            // 1 = ㄱ       // 2 3
            // 2 = ㄴ
            // 3 = 」

            if (currentMap == 0)
            {
                char[,] returnMap =
                {
                    { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                    { '#', ' ', '@', ' ', '#', '#', '#', '@', '#', '#'},
                    { '#', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                    { '$', '$', '$', '#', '#', ' ', '#', '#', '#', '#'},
                    { '$', '$', '$', '#', ' ', '@', '#', ' ', ' ', ' '},
                    { '$', '$', '$', '#', ' ', ' ', '#', ' ', '@', '#'},
                    { ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', '#', '#'},
                };
                return returnMap;
            }
            else if (currentMap == 1)
            {
                char[,] returnMap =
                {
                    { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                    { '#', ' ', ' ', ' ', 'w', 'W', 'w', 'W', '$', '#'},
                    { ' ', ' ', ' ', ' ', 'W', 'w', 'W', 'w', 'W', '#'},
                    { '#', '#', ' ', ' ', 'w', 'W', 'w', 'W', 'w', '#'},
                    { ' ', ' ', '#', ' ', 'W', 'w', 'W', 'w', 'W', '#'},
                    { '#', ' ', '@', '#', ' ', ' ', ' ', ' ', ' ', '#'},
                    { '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', '#'},
                };
                return returnMap;
            }
            else if (currentMap == 2)
            {
                char[,] returnMap =
                {
                    { ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', '#', '#'},
                    { ' ', ' ', '#', '#', ' ', ' ', '#', '@', ' ', ' '},
                    { ' ', '#', '#', ' ', ' ', ' ', ' ', '#', ' ', ' '},
                    { ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', '#'},
                    { '@', ' ', ' ', ' ', '&', '&', ' ', ' ', ' ', ' '},
                    { '#', ' ', ' ', '&', '&', '&', '&', ' ', ' ', ' '},
                    { '#', '#', '&', '&', '&', '&', '&', '&', '#', '#'},
                };
                return returnMap;
            }
            else
            {
                char[,] returnMap =
                {
                    { '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', '#'},
                    { ' ', '@', '#', '#', ' ', ' ', ' ', '#', '#', '#'},
                    { ' ', ' ', ' ', ' ', ' ', ' ', '#', '^', '~', '^'},
                    { '#', '#', '#', ' ', ' ', '#', '^', '~', '^', '~'},
                    { ' ', ' ', ' ', ' ', '#', '^', '~', '^', '~', '^'},
                    { ' ', ' ', ' ', '#', '^', '~', '^', '~', '^', '~'},
                    { '#', '#', '#', '#', '~', '^', '~', '^', '~', '^'},
                };
                return returnMap;
            }
        }

        static void SetTotalCoinCount() // 맵에 있는 '@', '$' 개수에 따라 totalCount 초기화
        {
            currentCoin = 0;
            // 맵에 '@' 를 찾아서 찾아야할 카운트 적용
            foreach (var value in map0)
            {
                if (value == '@' || value == '$')
                    totalCount++;
            }
            foreach (var value in map1)
            {
                if (value == '@' || value == '$')
                    totalCount++;
            }
            foreach (var value in map2)
            {
                if (value == '@' || value == '$')
                    totalCount++;
            }
            foreach (var value in map3)
            {
                if (value == '@' || value == '$')
                    totalCount++;
            }
        }

        static ref char[,] GetMap() // currentMap에 따른 char[,] 참조값 반환
        {
            if (currentMap == 0)
                return ref map0;
            else if (currentMap == 1)
                return ref map1;
            else if (currentMap == 2)
                return ref map2;
            else
                return ref map3;
        }

        static void Render(ref Position playerPos, ref char[,] map) // 맵 출력, 플레이어 출력
        {
            // 덧그리기
            Console.SetCursorPosition(0, 0);
            // 전부 지우기
            //Console.Clear();

            // 맵 그리기
            PrintMap(ref map);
            // 플레이어 그리기
            PrintPlayer(playerPos);
        }

        static void PrintMap(ref char[,] map) // 맵 출력
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < map.GetLongLength(0); y++)
            {
                for (int x = 0; x < map.GetLongLength(1); x++)
                {
                    char getTile = map[y, x];

                    // 타일에 따른 배경 색상 switch 문으로 처리
                    switch (getTile)
                    {
                        case '$':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 'w':
                        case 'W':
                            Console.ForegroundColor = getTile == 'w' ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                            break;
                        case '~':
                        case '^':
                            // Console.BackgroundColor = getTile == '~' ? ConsoleColor.Blue : ConsoleColor.DarkBlue; // 색을 섞어 쓰니까 잇아하네
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case '&':
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                    }

                    Console.Write(map[y, x]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }

        static void PrintPlayer(Position playerPos) // 플레이어 위치에 따른 출력
        {
            Console.SetCursorPosition(playerPos.x, playerPos.y);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('P');
            Console.ResetColor();

            Console.SetCursorPosition(0, 0);
        }

        static ConsoleKey Input() // 클라이언트 키입력 반환
        {
            return Console.ReadKey(true).Key; // 입력키 반환
        }

        static void Update(ConsoleKey key, ref Position playerPos, ref char[,] map) // 플레이어 액션, 무브, 코인체크
        {
            if (key == ConsoleKey.Spacebar)
                PlayerAction(playerPos, ref map);
            else
                PlayerMove(key, ref playerPos, ref map);

            // 클리어 체크(코인체크)
            if (CoinCheck())
                isClear = true;
        }

        static void PlayerMove(ConsoleKey key, ref Position playerPos, ref char[,] map) // 방향키 기능
        {
            Position targetPos = playerPos;

            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    // 키입력 행동
                    playerFront = ConsoleKey.LeftArrow;
                    targetPos.x--;
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    // 키입력 행동
                    playerFront = ConsoleKey.RightArrow;
                    targetPos.x++;
                    break;

                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    // 키입력 행동
                    playerFront = ConsoleKey.UpArrow;
                    targetPos.y--; // 반대로
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    // 키입력 행동
                    playerFront = ConsoleKey.DownArrow;
                    targetPos.y++;
                    break;
            }

            // 이동제한
            // 배열 초과 방지
            if (0 <= targetPos.y && targetPos.y < map.GetLength(0) &&
                0 <= targetPos.x && targetPos.x < map.GetLength(1))
            {
                char tileCheck = map[targetPos.y, targetPos.x];
                if (tileCheck == '#' || tileCheck == '@' || tileCheck == '&')
                {
                    return;
                }

                // 코인 쌓기
                if (tileCheck == '$')
                {
                    map[targetPos.y, targetPos.x] = ' ';
                    currentCoin++;
                }
            }

            // 맵 넘어가기
            switch (currentMap) // 맵의 끝 닿으면 이동
            {
                // 이동할 위치가 맵의 X끝이랑 비교후 맵이동
                // 이동할 위치가 맵의 Y끝이랑 비교후 맵이동
                case 0:     // 현재맵 0번(「)에서 이동
                    if (targetPos.x >= map.GetLength(1)) // 1번(ㄱ)
                    {
                        targetPos.x = 0; // 맵을 이동했다면 도착 위치도 정해져야됨
                        currentMap = 1;
                    }
                    else if (targetPos.y >= map.GetLength(0)) // 2번(ㄴ)
                    {
                        targetPos.y = 0;
                        currentMap = 2;
                    }
                    else if (targetPos.x < 0 || targetPos.y < 0) // 외곽 방지
                    {
                        return;
                    }
                    break;

                case 1:     // 현재맵 1번(ㄱ)에서 0번(「) or 3번(」)
                    if (targetPos.x <= -1) // 0번(「)
                    {
                        targetPos.x = map.GetLength(1) - 1;
                        currentMap = 0;
                    }
                    else if (targetPos.y >= map.GetLength(0)) // 3번(」)
                    {
                        targetPos.y = 0;
                        currentMap = 3;
                    }
                    else if (targetPos.x > map.GetLength(1) - 1 || targetPos.y < 0) // 외곽 방지
                    {
                        return;
                    }
                    break;

                case 2:     // 현재맵 2번(ㄴ)에서 0번(「) or 3번(」)
                    if (targetPos.x >= map.GetLength(1)) // 3번(」)
                    {
                        targetPos.x = 0;
                        currentMap = 3;
                    }
                    else if (targetPos.y <= -1) // 0번(「)
                    {
                        targetPos.y = map.GetLength(0) - 1;
                        currentMap = 0;
                    }
                    else if (targetPos.x < 0 || targetPos.y > map.GetLength(0) - 1) // 외곽 방지
                    {
                        return;
                    }
                    break;

                default:    // 현재맵 3번(」)에서 1번(ㄱ) or 2번(ㄴ)
                    if (targetPos.x <= -1) // 2번(ㄴ)
                    {
                        targetPos.x = map.GetLength(1) - 1;
                        currentMap = 2;
                    }
                    else if (targetPos.y <= -1) // 1번(ㄱ)
                    {
                        targetPos.y = map.GetLength(0) - 1;
                        currentMap = 1;
                    }
                    else if (targetPos.x > map.GetLength(1) - 1 || targetPos.y > map.GetLength(0) - 1) // 외곽 방지
                    {
                        return;
                    }
                    break;
            }

            PrintGameText();
            // 플레이어 위치 변경
            playerPos = targetPos;
        }

        static void PlayerAction(Position playerPos, ref char[,] map) // Spacebar 기능
        {
            // key는 스페이스바인 상태
            // 플레이어 앞에 @가 있으면 깨고 동전으로 변경

            Position targetPos = playerPos;

            switch (playerFront)
            {
                case ConsoleKey.LeftArrow:
                    targetPos.x--;
                    break;
                case ConsoleKey.RightArrow:
                    targetPos.x++;
                    break;
                case ConsoleKey.UpArrow:
                    targetPos.y--;
                    break;
                case ConsoleKey.DownArrow:
                    targetPos.y++;
                    break;
            }

            // 배열 초과 방지
            if (0 <= targetPos.y && targetPos.y < map.GetLength(0) &&
                0 <= targetPos.x && targetPos.x < map.GetLength(1))
            {
                char targetTile = map[targetPos.y, targetPos.x]; // 타일 체크용

                if (targetTile == '@') // @가 앞에 있으면
                {
                    map[targetPos.y, targetPos.x] = '$'; // 돈으로 변경
                }
            }
            else
                return;

        }

        static void PrintGameText() // 맵 오른쪽에 인게임 정보 출력
        {
            CurrentMapText();
            CoinText();
            GameKeyText();
        }

        static void CurrentMapText() // 현재 맵 위치
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(13, 1);
            Console.WriteLine($"현재 위치 : {currentMap + 1}번 맵   ");
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }

        static void CoinText() // 현재 코인 and 남은 코인
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(13, 2);
            Console.WriteLine($"현재 코인 : $ {currentCoin}개   ");

            Console.SetCursorPosition(13, 2);
            Console.WriteLine($"남은 코인 : $ {totalCount - currentCoin}개   ");
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }

        static void GameKeyText() // 맵 하단에 조작키
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("방향키 : 이동");
            Console.WriteLine("스페이스바 : 조사");
            Console.SetCursorPosition(0, 0);
        }

        static void TimeText() // 맵 오른쪽에 시간
        {
            Console.SetCursorPosition(13, 0);
            Console.WriteLine($"남은 시간  : {startTime}초  ");
            Console.SetCursorPosition(0, 0);
        }

        static bool CoinCheck() // currentCoin == totalCount면 true 반환
        {
            if (currentCoin == totalCount)
                return true;
            else
                return false;
        }

        static void End(bool isClear) // 게임 종료 
        {
            Console.Clear();
            if (isClear == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ"); // 16개
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ         클리어 ~~!         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ         종료 : ESC         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ"); // 16개
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ         실패  ㅠㅠ         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ         재시작 : R         ㅁ");
                Console.WriteLine("ㅁ         종료 : ESC         ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁ                            ㅁ");
                Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ");
                Console.ResetColor();
            }

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    RestartGame();
                    break;
                }
                else if (key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        static void RestartGame() // 게임 재시작용 초기화
        {
            // 모든 변수를 초기화
            isClear = false;
            currentMap = 0;
            currentCoin = 0;
            totalCount = 0;
            upTimer = 0;
            downTimer = 0;
            startTime = 0;

            // 맵 재설정
            map0 = MapInit(0);
            map1 = MapInit(1);
            map2 = MapInit(2);
            map3 = MapInit(3);

            // 메인함수 다시 실행
            Main();
        }
    }
}
