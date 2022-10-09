using System.Diagnostics;

internal class Program
{
    public static void Main(string[] args)
    {
        Stopwatch timer = new Stopwatch();
        string fileName = "a.bin";
        timer.Start();
        CreateFile(fileName, 1000000);
        Console.WriteLine("Файл перед сортуванням: ");
        OutputFile(fileName);
        StraightMergeSort sortObject = new StraightMergeSort(fileName);
        sortObject.Sort();
        Console.WriteLine("Файл після сортування: ");
        OutputFile(fileName);
        timer.Stop();
        Console.WriteLine($"Час виконання сортування: {(double)timer.ElapsedMilliseconds / 1000} секунд");
        Console.ReadKey();
    }

    public static void OutputFile(string file) 
    {
        using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
        {
            long length = reader.BaseStream.Length;
            long position = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (position == length)
                {
                    break;
                }
                else
                {
                    Console.Write($"{reader.ReadInt32()} ");
                    position += 4;
                }
            }
            Console.WriteLine();
        }
    }

    public static void CreateFile(string fileName, int count) 
    {
        using (BinaryWriter generator = new BinaryWriter(File.Create(fileName)))
        {
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                generator.Write(rnd.Next(-500, 500));
            }
        }
    }

    public class StraightMergeSort
    {
        public string FileInput { get; set; }
        private long iterations, cortaige;
        public StraightMergeSort(string input)
        {
            FileInput = input;
            iterations = 1; 
        }
        public void Sort()
        {
            while (true)
            {
                Split();
                if (cortaige == 1)
                {
                    break;
                }
                Merge();
            }
            Console.WriteLine();
        }
        private void Merge()
        {
            using (BinaryReader readerA = new BinaryReader(File.OpenRead("b.bin")))
            using (BinaryReader readerB = new BinaryReader(File.OpenRead("c.bin")))
            using (BinaryWriter bw = new BinaryWriter(File.Create(FileInput)))
            {
                long counterA = iterations, counterB = iterations;
                int elementA = 0, elementB = 0;
                bool pickedA = false, pickedB = false, endA = false, endB = false;
                long lengthA = readerA.BaseStream.Length;
                long lengthB = readerB.BaseStream.Length;
                long positionA = 0;
                long positionB = 0;
                while (!endA || !endB)
                {
                    if (counterA == 0 && counterB == 0)
                    {
                        counterA = iterations;
                        counterB = iterations;
                    }
                    if (positionA != lengthA)
                    {
                        if (counterA > 0 && !pickedA)
                        {
                            elementA = readerA.ReadInt32();
                            positionA += 4;
                            pickedA = true;
                        }
                    }
                    else
                    {
                        endA = true;
                    }

                    if (positionB != lengthB)
                    {
                        if (counterB > 0 && !pickedB)
                        {
                            elementB = readerB.ReadInt32();
                            positionB += 4;
                            pickedB = true;
                        }
                    }
                    else
                    {
                        endB = true;
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            if (elementA < elementB)
                            {
                                bw.Write(elementA);
                                counterA--;
                                pickedA = false;
                            }
                            else
                            {
                                bw.Write(elementB);
                                counterB--;
                                pickedB = false;
                            }
                        }
                        else
                        {
                            bw.Write(elementA);
                            counterA--;
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        bw.Write(elementB);
                        counterB--;
                        pickedB = false;
                    }
                }
                iterations *= 2;
            }
        }
        private void Split() 
        {
            cortaige = 1;
            using (BinaryReader br = new BinaryReader(File.OpenRead(FileInput)))
            using (BinaryWriter writerA = new BinaryWriter(File.Create("b.bin")))
            using (BinaryWriter writerB = new BinaryWriter(File.Create("c.bin")))
            {
                long counter = 0;
                bool flag = true;
                long length = br.BaseStream.Length;
                long position = 0;
                while (position != length)
                {
                    if (counter == iterations)
                    {
                        flag = !flag;
                        counter = 0;
                        cortaige++;
                    }
                    int currentElement = br.ReadInt32();
                    position += 4;
                    if (flag)
                    {
                        writerA.Write(currentElement);
                    }
                    else
                    {
                        writerB.Write(currentElement);
                    }
                    counter++;
                }
            }
        }

    }
}