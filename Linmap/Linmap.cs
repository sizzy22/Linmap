using System.Globalization;

namespace Linmap
{
    public sealed class Linmap
    {
        private byte[] map;
        private int bytesPerPixel = 4;
        private int width = 0;
        private int height = 0;
        public int Width { get => width; }
        public int Height { get => height; }
        public int BitsPerPixel { get => bytesPerPixel * 8; }

        public Linmap(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new byte[height * width * bytesPerPixel];
        }

        public Linmap(int width, int height, Color c) : this(width, height)
        {
            this.width = width;
            this.height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = (y * width + x) * bytesPerPixel;
                    map[i] = c.R;
                    map[i + 1] = c.G;
                    map[i + 2] = c.B;
                    if (bytesPerPixel == 4) map[i + 3] = c.A;
                }
            }
        }

        public Linmap(string filePath)
        {
            LoadMap(filePath);
        }

        public void LoadMap(string filePath)
        {
            using(BinaryReader binaryreader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                binaryreader.BaseStream.Seek(10, SeekOrigin.Begin);
                int offset = binaryreader.ReadInt32();
                binaryreader.BaseStream.Seek(18, SeekOrigin.Begin);
                width = binaryreader.ReadInt32();
                height = binaryreader.ReadInt32();

                binaryreader.BaseStream.Seek(28, SeekOrigin.Begin);
                int bitsPerPixel = binaryreader.ReadInt16();
                bytesPerPixel = bitsPerPixel / 8;
                if (bitsPerPixel != 24 && bitsPerPixel != 32) throw new Exception($"This library support just 24 and 32 bits files {bitsPerPixel}");
                
                map = new byte[width * height * bytesPerPixel];
                int bytesPerRow = width * bytesPerPixel;
                int rowbuffer = (4 - (bytesPerRow % 4)) % 4;
                binaryreader.BaseStream.Seek(offset, SeekOrigin.Begin);
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int i = (y * width + x) * bytesPerPixel;
                        //B
                        map[i + 2] = binaryreader.ReadByte();
                        //G
                        map[i + 1] = binaryreader.ReadByte();
                        //R
                        map[i] = binaryreader.ReadByte();
                        
                        
                        //A
                        if (bytesPerPixel == 4) map[i + 3] = binaryreader.ReadByte();

                        
                    }

                    binaryreader.ReadBytes(rowbuffer);
                }
            }
        }

        public void SaveMap(string path)
        {
            int rowSize = ((width * (bytesPerPixel * 8) + 31) / 32) * 4;
            int ImageSize = rowSize * height;
            int padding = (4 - (width * bytesPerPixel) % 4) % 4;
            Int32 filesize = 54 + height * (width * bytesPerPixel + padding);
            using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                bw.Write((byte)'B');
                bw.Write((byte)'M');
                bw.Write(filesize);
                bw.Write(new Int32());
                bw.Write((Int32)54);
                bw.Write((Int32)40);
                bw.Write((Int32)width);
                bw.Write((Int32)height);
                bw.Write((Int16)1);
                bw.Write((short)(bytesPerPixel * 8));
                bw.Write((Int32)0);
                bw.Write((Int32)ImageSize);
                bw.Write((Int32)0);
                bw.Write((Int32)0);
                bw.Write((Int32)0);
                bw.Write((Int32)0);

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int i = (y * width + x) * bytesPerPixel;
                        //B
                        bw.Write(map[i + 2]);
                        //G
                        bw.Write(map[i + 1]);
                        //R
                        bw.Write(map[i]);
                        //A
                        if (bytesPerPixel == 4) bw.Write(map[i + 3]);
                    }

                    if (bytesPerPixel == 3)
                        for (int p = 0; p < padding; p++)
                            bw.Write((byte)0);
                }


            }
        }

        public Color GetPixel(int x, int y)
        {
            int i = (y * width + x) * bytesPerPixel;
            return bytesPerPixel == 3 ? Color.FromArgb(map[i], map[i + 1], map[i + 2]) : Color.FromArgb(map[i + 3], map[i], map[i + 1], map[i + 2]);
        }
        public void SetPixel(int x, int y, Color c)
        {
            int i = (y * width + x) * bytesPerPixel;
            map[i] = c.R;
            map[i + 1] = c.G;
            map[i + 2] = c.B;
            if (bytesPerPixel == 4) map[i + 3] = c.A;
        }
        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            int i = (y * width + x) * bytesPerPixel;
            map[i] = r;
            map[i + 1] = g;
            map[i + 2] = b;
            if (bytesPerPixel == 4) map[i + 3] = 255;
        }
        public void SetPixel(int x, int y, byte r, byte g, byte b,byte a)
        {
            int i = (y * width + x) * bytesPerPixel;
            map[i] = r;
            map[i + 1] = g;
            map[i + 2] = b;
            if (bytesPerPixel == 4) map[i + 3] = a;
        }

        public float PPIResolution(int inch) => (float)Math.Sqrt(height * height + width * width) / inch;
    }
}
