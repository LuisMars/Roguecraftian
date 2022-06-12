namespace Roguecraft.Engine.Random;

public abstract class HashFunction : IRandom
{
    public int[] GausianValuesWithMean(int numberOfValues, int mean, float stdDev, int data)
    {
        var vals = new int[numberOfValues];
        for (int i = 0; i < numberOfValues; i++)
        {
            vals[i] = mean;
        }
        for (int i = 0; i < numberOfValues; i++)
        {
            double u1 = 1.0 - Value(data * i * vals[Math.Max(0, i - 1)]); //uniform(0,1] random doubles
            double u2 = 1.0 - Value((int)u1 * data * i * vals[Math.Max(0, i - 1)]);
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            var currentMean = vals.Sum() / 3.0;
            var divergence = mean - currentMean;
            var modifiedMean = mean + divergence;
            var modifiedStdDev = stdDev - divergence * divergence;
            double randNormal = modifiedMean + modifiedStdDev * randStdNormal; //random normal(mean,stdDev^2)

            vals[i] = (int)Math.Round(randNormal);
            currentMean = vals.Sum() / 3.0;
            divergence = mean - currentMean;
            var rounded = (int)Math.Ceiling(randNormal);
            if (divergence < 0)
            {
                rounded = (int)Math.Floor(randNormal);
            }
            vals[i] = rounded;
        }
        return vals;
    }

    public int GausianValueWithMean(int mean, float stdDev, int data)
    {
        return GausianValuesWithMean(1, mean, stdDev, data)[0];
    }

    // Main hash function for any number of parameters.
    public abstract uint GetHash(params int[] data);

    // Optional optimizations for few parameters.
    // Derived classes can optimize with custom implementations.
    public virtual uint GetHash(int data)
    {
        return GetHash(new int[] { data });
    }

    public virtual uint GetHash(int x, int y)
    {
        return GetHash(new int[] { x, y });
    }

    public virtual uint GetHash(int x, int y, int z)
    {
        return GetHash(new int[] { x, y, z });
    }

    public int Range(int min, int max, params int[] data)
    {
        if (min == max)
        {
            return min;
        }
        return min + (int)(GetHash(data) % (max - min));
    }

    // Potentially optimized overloads for few parameters.
    public int Range(int min, int max, int data)
    {
        if (min == max)
        {
            return 0;
        }
        return min + (int)(GetHash(data) % (max - min));
    }

    public int Range(int min, int max, int x, int y)
    {
        if (min == max)
        {
            return min;
        }
        return min + (int)(GetHash(x, y) % (max - min));
    }

    public int Range(int min, int max, int x, int y, int z)
    {
        return min + (int)(GetHash(x, y, z) % (max - min));
    }

    public float Range(float min, float max, params int[] data)
    {
        return min + GetHash(data) * (float)(max - min) / uint.MaxValue;
    }

    // Potentially optimized overloads for few parameters.
    public float Range(float min, float max, int data)
    {
        return min + GetHash(data) * (float)(max - min) / uint.MaxValue;
    }

    public float Range(float min, float max, int x, int y)
    {
        return min + GetHash(x, y) * (float)(max - min) / uint.MaxValue;
    }

    public float Range(float min, float max, int x, int y, int z)
    {
        return min + GetHash(x, y, z) * (float)(max - min) / uint.MaxValue;
    }

    public float Value(params int[] data)
    {
        return GetHash(data) / (float)uint.MaxValue;
    }

    // Potentially optimized overloads for few parameters.
    public float Value(int data)
    {
        return GetHash(data) / (float)uint.MaxValue;
    }

    public float Value(int x, int y)
    {
        return GetHash(x, y) / (float)uint.MaxValue;
    }

    public float Value(int x, int y, int z)
    {
        return GetHash(x, y, z) / (float)uint.MaxValue;
    }
};