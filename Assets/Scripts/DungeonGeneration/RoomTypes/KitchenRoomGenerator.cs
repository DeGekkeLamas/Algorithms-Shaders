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
        public float counterDoorDistance;

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
            Vector2 counterSizeVec = new Vector2(counterSize, counterSize);

            for (int i = 0; i < d.rooms.Count; i++) // for each room
            {
                RectInt room = d.rooms[i];

                // Skip origin room
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
                        Vector3 position = new(room.center.x + offset.x, counterSize * .5f, room.center.y + offset.y);
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
                Vector2[] doorDirs = d.GetDoorDirections(room);
                Vector2 side = Vector2.right;
                Vector2 usedSize = room.size - 2 * counterSizeVec;
                Vector2 usedMin = room.min + counterSizeVec;
                for (int j = 0; j < 4; j++)
                {
                    side = VectorMath.RotateVectorXY(side, 90);
                    Vector2 ZeroToOneSide = MathTools.Vector3Max(side, Vector2.zero);
                    // Skip some sides
                    //if (d.random.Next(0, 2) > 1) continue;

                    // Get positions
                    Vector2 startPos = usedMin + (Vector2)MathTools.Vector3Multiply(usedSize, ZeroToOneSide);
                    float lengthOtherside = MathTools.Vector3CompSum(MathTools.Vector3Multiply(usedSize, Vector3.one - MathTools.Vector3Abs(side)) );

                    DebugExtension.DebugWireSphere(new(startPos.x,0,startPos.y), Color.red, 1, 25);

                    // Spawn counters
                    for (int k = 0; k < lengthOtherside / counterSize; k++)
                    {
                        float sideOffset = k * counterSize;
                        Vector3 placePos = startPos + (new Vector2(Mathf.Abs(side.y), Mathf.Abs(side.x)) * sideOffset);

                        // Do not spawn if theres a door there
                        if (IntersectWithPoints(placePos - (Vector3)room.center, doorDirs, counterDoorDistance)) continue;

                        placePos = new(placePos.x, counterSize * .5f, placePos.y);
                        // Spawn counter
                        SpawnCounter(placePos, Quaternion.LookRotation(-new Vector3(side.x,0,side.y), Vector3.up), counterContainer.transform);
                        yield return d.interval;
                    }
                }

            }
            yield return new();
        }

        /// <summary>
        /// Check if there is a door in that direction, using angle between positions
        /// </summary>
        static bool IntersectWithPoints(Vector2 point, Vector2[] points, float minAngle)
        {
            foreach (var dir in points)
            {
                float angle = VectorMath.GetAngleBetweenVectors(dir, point);
                Debug.Log($"Doorpos = {dir}, point = {point}, angle = {angle}");
                if (angle < minAngle)
                {
                    Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    //Debug.Break();
                    return true;
                }
            }
            return false;
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
