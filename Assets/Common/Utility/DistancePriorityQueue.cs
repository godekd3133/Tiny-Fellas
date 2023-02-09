using System.Collections.Generic;
using UnityEngine;

public class DistancePriorityQueue
{
    struct LocationDatas
    {
        public Transform transform;
        public float distance;

        public LocationDatas(Transform transform, float distance)
        {
            this.transform = transform;
            this.distance = distance;
        }
    }

    private List<LocationDatas> nearestDistanceHeap;
    private Vector3 standardPos;

    public Vector3 StandardPos
    {
        get => standardPos;
    }

    public int Count
    {
        get => nearestDistanceHeap.Count;
    }

    public Transform Peek
    {
        get
        {
            if (Count > 0)
                return nearestDistanceHeap[0].transform;
            return null;
        }
    }


    public DistancePriorityQueue(Vector3 standard, int capacity = 100)
    {
        standardPos = standard;
        nearestDistanceHeap = new List<LocationDatas>(capacity);
    }


    public void Clear()
    {
        nearestDistanceHeap.Clear();
    }

    public void Enqueue(Transform pos)
    {
        float distance = (standardPos - pos.position).magnitude;
        LocationDatas element = new LocationDatas(pos, distance);
        nearestDistanceHeap.Add(element);
        int idx = Count - 1;

        while (idx > 0 && idx < Count)
        {
            int parentNodeIdx = (idx - 1) / 2;
            float parentsValue = nearestDistanceHeap[parentNodeIdx].distance;
            float childValue = nearestDistanceHeap[idx].distance;

            if (childValue < parentsValue)
            {
                SwapChildAndParents(idx);
                idx = parentNodeIdx;
            }
            else
                break;
        }
    }

    private void SortBottomUp(int startIdx)
    {
        while (startIdx > 0 && startIdx < Count)
        {
            int parentNodeIdx = (startIdx - 1) / 2;
            float parentsValue = nearestDistanceHeap[parentNodeIdx].distance;
            float childValue = nearestDistanceHeap[startIdx].distance;

            if (childValue < parentsValue)
            {
                SwapChildAndParents(startIdx);
                startIdx = parentNodeIdx;
            }
            else
                break;
        }
    }

    public Transform Dequeue()
    {
        if (Count < 1)
            return null;
        else if (Count == 1)
        {
            Transform buffer = nearestDistanceHeap[0].transform;
            nearestDistanceHeap.RemoveAt(0);
            return buffer;
        }

        int idx = 0;

        Transform returnValue = nearestDistanceHeap[idx].transform;
        nearestDistanceHeap[idx] = nearestDistanceHeap[Count - 1];
        nearestDistanceHeap.RemoveAt(Count - 1);

        SortTopDown(idx);
        return returnValue;
    }

    private void SortTopDown(int startIdx)
    {
        bool isSwaped = true;

        while (startIdx < Count && isSwaped)
        {
            isSwaped = false;
            int parentIdx = startIdx;
            int leftChildIdx = startIdx * 2 + 1;
            if (leftChildIdx >= Count)
                return;

            bool hasRightChild = false;
            int rightChildIdx = leftChildIdx + 1;
            if (rightChildIdx < Count)
                hasRightChild = true;

            bool isLeftChildSmaller;
            if (hasRightChild)
                isLeftChildSmaller = nearestDistanceHeap[leftChildIdx].distance <
                                     nearestDistanceHeap[rightChildIdx].distance;
            else
                isLeftChildSmaller = true;

            if (isLeftChildSmaller)
            {
                if (IsParentLargerThan(leftChildIdx))
                {
                    SwapChildAndParents(leftChildIdx);
                    startIdx = leftChildIdx;
                    isSwaped = true;
                }
                else if (hasRightChild && IsParentLargerThan(rightChildIdx))
                {
                    SwapChildAndParents(rightChildIdx);
                    startIdx = rightChildIdx;
                    isSwaped = true;
                }
            }
            else
            {
                if (IsParentLargerThan(rightChildIdx))
                {
                    SwapChildAndParents(rightChildIdx);
                    startIdx = rightChildIdx;
                    isSwaped = true;
                }
                else if (IsParentLargerThan(leftChildIdx))
                {
                    SwapChildAndParents(leftChildIdx);
                    startIdx = leftChildIdx;
                    isSwaped = true;
                }
            }
        }
    }

    private bool IsParentLargerThan(int childIdx)
    {
        int parentIdx = (childIdx - 1) / 2;
        return nearestDistanceHeap[parentIdx].distance > nearestDistanceHeap[childIdx].distance;
    }

    private void SwapChildAndParents(int childIdx)
    {
        int parentIdx = (childIdx - 1) / 2;
        LocationDatas buffer = nearestDistanceHeap[parentIdx];
        nearestDistanceHeap[parentIdx] = nearestDistanceHeap[childIdx];
        nearestDistanceHeap[childIdx] = buffer;
    }

    public void ChangeStanrdardPosAndInit(Vector3 newStandard)
    {
        standardPos = newStandard;
        nearestDistanceHeap.Clear();
    }

    public void ChangeStandardPos(Vector3 newStandard)
    {
        LocationDatas[] buffer = nearestDistanceHeap.ToArray();
        ChangeStanrdardPosAndInit(newStandard);
        foreach (var a in buffer)
            Enqueue(a.transform);
    }

    public bool IsContain(Transform transform)
    {
        for (int i = 0; i < nearestDistanceHeap.Count; i++)
            if (nearestDistanceHeap[i].transform == transform)
                return true;
        return false;
    }

    public Transform this[int i]
    {
        get => nearestDistanceHeap[i].transform;
    }
}

