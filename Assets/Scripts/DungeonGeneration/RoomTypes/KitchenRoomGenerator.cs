using InventoryStuff;
using System;
using System.Collections;
using UnityEngine;
namespace DungeonGeneration
{
    public class KitchenRoomGenerator : RoomAssetGenerator
    {
        [Header("Type specific")]
        public GameObject counterMiddle;
        public GameObject counterCornerL;
        public GameObject counterCornerR;
        public int counterLength = 4;

        public ItemLootDrop[] counterReplacements;

        public override IEnumerator SpawnObjects()
        {
            // Containers
            roomAssetContainer = new("RoomAssetContainer");
            roomAssetContainer.transform.parent = d.assetContainer.transform;
            itemSpawnsContainer = new("ItemSpawnsContainer");
            itemSpawnsContainer.transform.parent = roomAssetContainer.transform;

            GameObject counterContainer = new("CounterContainer");
            counterContainer.transform.parent = roomAssetContainer.transform;

            for (int i = 0; i < d.rooms.Count; i++)
            {
                RectInt room = d.rooms[i];

                // SKip origin room
                if (room == d.GetOriginRoom()) continue;

                // Kitchen island
                Vector2 offset = new(-.5f * counterLength, -0.5f);
                bool roomvertical = room.height > room.width;
                float counterSize = counterMiddle.transform.lossyScale.x;

                if (roomvertical) offset = new(offset.y, offset.x);
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < counterLength; k++)
                    {
                        var rotation = j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : Quaternion.Euler(0, !roomvertical ? 180 : 270, 0);
                        Transform counter = Instantiate(counterMiddle, new(
                            room.center.x + offset.x, 
                            0 + counterSize * .5f, 
                            room.center.y + offset.y), 
                            rotation, counterContainer.transform).transform;
                        if (roomvertical) offset.y += counterSize;
                        else offset.x += counterSize;

                        // Replace with sink
                        //if (d.GetSeed().Next(0,100) < sinkProbabiliy)
                        //{
                        //    Instantiate(sink, counter.position + sink.transform.position, counter.rotation, counterContainer.transform);
                        //    Destroy(counter.gameObject);
                        //}
                        //else // Item pickup
                        //{
                        //    SpawnPickup(new(counter.position.x, 10, counter.position.z));
                        //}
                    }
                    if (roomvertical) offset = new(offset.x + counterSize, -.5f * counterLength);
                    else offset = new(-.5f * counterLength, offset.y + counterSize);
                }
            }
            yield return new();
        }
    }
}
