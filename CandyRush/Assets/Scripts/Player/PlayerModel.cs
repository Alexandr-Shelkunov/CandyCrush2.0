using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerModel : MonoBehaviour
    {
        public float DistanceScore { get; set; }
        public int CandyCount { get; set; }
        public float Weight { get; set; }

        public PlayerModel()
        {
            // ������������� ��������
            DistanceScore = 0f;
            CandyCount = 0;
            Weight = 40f;
        }

        // ������ ��� �������������� � ������� ������, ���� ��� ����
        public void ResetStats()
        {
            CandyCount = 0;
            Weight = 40f;
        }
    }
