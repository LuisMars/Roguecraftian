namespace Roguecraft.Engine.Random;

public interface IRandom
{
    int[] GausianValuesWithMean(int numberOfValues, int mean, float stdDev, int data);

    int GausianValueWithMean(int mean, float stdDev, int data);

    uint GetHash(int data);

    uint GetHash(int x, int y);

    uint GetHash(int x, int y, int z);

    uint GetHash(params int[] data);

    float Range(float min, float max, int data);

    float Range(float min, float max, int x, int y);

    float Range(float min, float max, int x, int y, int z);

    float Range(float min, float max, params int[] data);

    int Range(int min, int max, int data);

    int Range(int min, int max, int x, int y);

    int Range(int min, int max, int x, int y, int z);

    int Range(int min, int max, params int[] data);

    float Value(int data);

    float Value(int x, int y);

    float Value(int x, int y, int z);

    float Value(params int[] data);
}