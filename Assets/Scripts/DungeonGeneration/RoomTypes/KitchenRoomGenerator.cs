using InventoryStuff;
using NaughtyAttributes;
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

        [ReadOnly] public int totalReplaceChance;
        public ItemLootDrop<GameObject>[] counterReplacements;

        private void OnValidate()
        {
            ItemLootDrop<GameObject>.MassValidate(counterReplacements);
            totalReplaceChance = ItemLootDrop<GameObject>.GetTotalItemProbability(counterReplacements);
        }

        public override IEnumerator SpawnObjects()
        {
            GameObject counterContainer = new("CounterContainer");
            counterContainer.transform.parent = roomAssetContainer.transform;

            float counterSize = counterMiddle.transform.lossyScale.x;

            for (int i = 0; i < d.rooms.Count; i++) // for each room
            {
                RectInt room = d.rooms[i];

                // SKip origin room
                if (room == d.originRoom) continue;

                // Kitchen island
                Vector2 offset = new(-.5f * counterLength, -0.5f);
                bool roomvertical = room.height > room.width;

                if (roomvertical) offset = new(offset.y, offset.x);
                // Center
                for (int j = 0; j < 2; j++)
                {
                    // Counter pieces
                    for (int k = 0; k < counterLength; k++)
                    {
                        Vector3 position = new(room.center.x + offset.x, 0 + counterSize * .5f, room.center.y + offset.y);
                        Quaternion rotation = j == 1 ? Quaternion.Euler(0, !roomvertical ? 0 : 90, 0) : 
                            Quaternion.Euler(0, !roomvertical ? 180 : 270, 0);

                        SpawnCounter(position, rotation, counterContainer.transform);
                        if (roomvertical) offset.y += counterSize;
                        else offset.x += counterSize;
                        yield return d.interval;
                    }
                    if (roomvertical) offset = new(offset.x + counterSize, -.5f * counterLength);
                    else offset = new(-.5f * counterLength, offset.y + counterSize);
                }

                // Edges
                for (int j = 0; j < 4; j++)
                {
                    // Skip some sides
                    if (d.random.Next(0, 2) > 1) continue;
                    
                }

            }
            yield return new();
        }

        void SpawnCounter(Vector3 position, Quaternion rotation, Transform container = null)
        {
            Transform counter = Instantiate(counterMiddle, position,
                rotation, container).transform;

            //Replace counter
            GameObject replacement = ItemLootDrop<GameObject>.GetItemFromLoottable(counterReplacements, d.random);
            if (replacement != null)
            {
                Instantiate(replacement, counter.position + VectorMath.RotateVectorXZ(replacement.transform.position, -counter.eulerAngles.y - 90)
                    , counter.rotation, container);
                Destroy(counter.gameObject);
            }
            else // Item pickup
            {
                SpawnPickup(new(counter.position.x, 10, counter.position.z));
            }
        }
    }
}
