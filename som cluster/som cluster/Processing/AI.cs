using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Bean
{
    class AI
    {
        Bitmap bitmap;
        int[,,] color;

        public AI(Bitmap orignalBitamp)
        {
            this.bitmap = orignalBitamp;
            color = new int[3, orignalBitamp.Width, orignalBitamp.Height];

            for (int x = 0; x < orignalBitamp.Width; x++)
                for (int y = 0; y < orignalBitamp.Height; y++)
                {
                    color[0, x, y] = orignalBitamp.GetPixel(x, y).R;
                    color[1, x, y] = orignalBitamp.GetPixel(x, y).G;
                    color[2, x, y] = orignalBitamp.GetPixel(x, y).B;
                }
        }

        #region
        /* 
        K-Means
        비지도 학습의 기본이자 대표적인 알고리즘
        
        Step #1 클러스터 개수를 정해줍니다
        Step #2 클러스터의 중심 좌표를 정적으로 할당 해줍니다.
        Step #3 클러스터 범위 기준으로 중심좌표를 설정합니다 [중심좌표 =시그마(색상*개수)/시그마(개수)]
        Step #4 중심좌표가 이전 좌표와 같은지 확인합니다
                다르면 3번으로 가서 반복합니다.
     
        */
        public void Kmeans()
        {
            //Step #1 클러스터 개수를 정해줍니다
            int   clusterNum = 3;                     //클러스터 개수
            int[] centerNum  = new int[clusterNum];   //중심 좌표
            int[] colorCount = new int[256];          //256가지 색상의 개수 [EX 234번의 색깔은 4365개가 있다]
            int[,] sumCount  = new int[clusterNum,2]; //중심좌표를 구하기 위한 2차원 배열
                                                      //0 = 분모 (색상의 개수)
                                                      //1 = 분자 (색상 * 색상의 개수)
            int option = 0;                           //반복문 종료

            //Step #2 클러스터의 중심 좌표를 정적으로 할당 해줍니다. [255는 색상의 최대값]
            for (int i = 0; i < clusterNum; i++)
            {
                int h = 255 / clusterNum;
                centerNum[i] = h + h * i;
            }
            
            //컬러 색상의 개수를 구함
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    colorCount[color[0, x, y]]++;
                }
            
            do
            {
                //Step #3 클러스터 범위 기준으로 중심좌표를 설정합니다 [중심좌표 =시그마(색상*개수)/시그마(개수)]
                for (int i = 0; i < 256; i++) //256은 색상의 최대값 [0~255]
                {
                    int sum = i * colorCount[i]; //개수랑 색상의 곱
                    for (int j = 0; j < clusterNum ; j++) {
                        int startPoint = j == 0            ? 0   : (centerNum[j - 1] + centerNum[j]) / 2;//0 보다 작으면 0
                        int endPoint   = j == clusterNum-1 ? 255 : (centerNum[j] + centerNum[j + 1]) / 2;//클러스터개수 보다 크면 255
                        if (startPoint <= i && i <= endPoint)
                        {
                            sumCount[j, 0] = sumCount[j, 0] + colorCount[i];
                            sumCount[j, 1] = sumCount[j, 1] + sum;
                        }
                    }
                }

                // Step #4 중심좌표가 이전 좌표와 같은지 확인합니다
                //다르면 3번으로 가서 반복합니다.
                for (int i = 0; i < clusterNum; i++)
                {
                    if (centerNum[i] == sumCount[i, 1] / sumCount[i, 0])
                    {
                        option++;
                    }
                    else
                    {
                        option = 0;
                        centerNum[i] = sumCount[i, 1] / sumCount[i, 0];
                    }
                }
            } while (option <= clusterNum);
            
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                    for (int j = 0; j < clusterNum; j++)
                    {
                        int startPoint = j == 0              ? 0   : (centerNum[j - 1] + centerNum[j]) / 2;//0 보다 작으면 0
                        int endPoint   = j == clusterNum - 1 ? 255 : (centerNum[j] + centerNum[j + 1]) / 2;//클러스터개수 보다 크면 255
                        if (startPoint <= color[0, x, y] && color[0, x, y] <= endPoint)
                        {
                            color[0, x, y] = centerNum[j];
                            color[1, x, y] = centerNum[j];
                            color[2, x, y] = centerNum[j];
                        }
                    }
        }
        #endregion

        public Bitmap getBitmap()
        {
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(color[0, x, y], color[1, x, y], color[2, x, y]));
                }
            return bitmap;
        }

    }
}
