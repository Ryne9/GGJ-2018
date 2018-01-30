using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Type { ferrium, resonite, chlorophate, neosteel, vibranium, photonite, unobtanium, adamantite, mythrill};
    public class Inventory
    {
        int maxResources;
        int maxBlips;
        int currentBlips;
        int currentResourceCount;
        IDictionary<int, int> inv;
        
        public Inventory(int maxResources, int maxBlips)
        {
            this.maxResources = maxResources;
            this.maxBlips = maxBlips;
            currentBlips = maxBlips / 2;
            currentResourceCount = 0;
            inv = new Dictionary<int, int>();
            for (int i = 0; i <= 8; i++)
            {
                inv.Add(i, 0);
            }
        }

        public bool AddResource(int itemType)
        {
            if (currentResourceCount == maxResources)
                return false;
            inv[itemType] = inv[itemType] + 1;
            currentResourceCount++;
            return true;
        }

        public bool AddBlips(int blips)
        {
            if (currentBlips == maxBlips)
                return false;
            else if (currentBlips + blips >= maxBlips)
                currentBlips = maxBlips;
            else if (currentBlips + blips < 0)
                currentBlips = 0;
            else
                currentBlips += blips;
            return true;
        }

        public int GetBlips()
        {
            return currentBlips;
        }

        public int GetResourceCount(int resource)
        {
            return inv[resource];
        }

        public void RemoveResource(int resourceType)
        {
            inv[resourceType] -= 1;
        }

        override
        public string ToString()
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                output += i + " " + inv[i] + " ";
            }
            return output;
        }
    }
}
