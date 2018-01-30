using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Mission : IComparable
    {
        int[] checklist;
        float timeLeft;

        public Mission(int[] itemTypes)
        {
            checklist = itemTypes;
            timeLeft = 60f;
        }

        public bool CanBeCompleted(int[] request)
        {
            int[] dummy = (int[]) checklist.Clone();
            if (request.Length < checklist.Length)
                return false;
            foreach (int item in request)
            {
                if (dummy.Contains(item))
                    dummy[Array.IndexOf(dummy, item)] = -1;
            }
            foreach (int item in dummy)
            {
                if (item >= 0)
                    return false;
            }
            return true;
        }

        public int[] GetChecklist()
        {
            return checklist;
        }

        public void DecrementTime(float d)
        {
            timeLeft -= d;
        }

        public float GetTimeLeft()
        {
            return timeLeft;
        }

        public int CompareTo(object a)
        {
            Mission m1 = (Mission)a;
            Mission m2 = this;

            int m1Difficulty = getDifficulty(m1.GetChecklist()[0]) + getDifficulty(m1.GetChecklist()[1]);
            int m2Difficulty = getDifficulty(m2.GetChecklist()[0]) + getDifficulty(m2.GetChecklist()[1]);

            if (m1Difficulty > m2Difficulty)
            {
                return -1;
            }
            else if (m1Difficulty < m2Difficulty)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private int getDifficulty(int type)
        {
            return type / 3;
        }
    }
}
