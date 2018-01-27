using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Mission
    {
        int[] checklist;
        float timeLeft;

        public Mission(int[] itemTypes)
        {
            checklist = itemTypes;
            timeLeft = 30f;
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
    }
}
