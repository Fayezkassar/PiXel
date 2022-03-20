using ImageResizingApp.Models.Interfaces;
using System;
using System.Text;
using System.Linq;
using Oracle.ManagedDataAccess.Types;
using ImageMagick;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public ITable Table { get; set; }

        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }

        private readonly OracleConnection _connection;

        public OracleColumn(ITable table, OracleConnection connection)
        {
            this.Table = table;
            _connection = connection;
        }
        public bool Resize(int? from, int? to, int? minSize, int? maxSize)
        {
            //OracleTransaction transaction;
            //OracleCommand command = _connection.CreateCommand();
            //transaction = _connection.BeginTransaction();
            //command.Transaction = transaction;
            try
            {
                var _from = from ?? 0;
                string sql = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM " + Table.Name + " WHERE NIDOC=103896";
                if (to != null)
                {
                    sql += " AND ROWNUM<=" + to;
                }
                OracleCommand cmd = new OracleCommand(sql, _connection);
                OracleDataReader dr = cmd.ExecuteReader();
                List<string> pKs = new List<string>();
                int n = Table.PrimaryKeys.Count();

                while (dr.Read())
                {
                    for (int i = 0; i < n; i++)
                    {
                        pKs.Add(dr.GetString(i));
                    }
                    OracleBlob blob = dr.GetOracleBlob(n);
                    byte[] bytes = new byte[blob.Length];
                    blob.Read(bytes, 0, (int)blob.Length);
                    MagickImage img = new MagickImage(bytes);
                    MagickImage img1 = new MagickImage(bytes);
                    MagickImage img2 = new MagickImage(bytes);
                    MagickImage img3 = new MagickImage(bytes);
                    MagickImage img4 = new MagickImage(bytes);
                    MagickImage img5 = new MagickImage(bytes);
                    MagickImage img6 = new MagickImage(bytes);
                    MagickImage img7 = new MagickImage(bytes);
                    MagickImage img8 = new MagickImage(bytes);
                    MagickImage img9 = new MagickImage(bytes);
                    MagickImage img10 = new MagickImage(bytes);
                    MagickImage img11 = new MagickImage(bytes);
                    MagickImage img12 = new MagickImage(bytes);
                    MagickImage img13 = new MagickImage(bytes);
                    img.Resize(400,600);
                    img1.InterpolativeResize(400,600, PixelInterpolateMethod.Spline);
                    img2.InterpolativeResize(400, 600, PixelInterpolateMethod.Bilinear);
                    img3.InterpolativeResize(400, 600, PixelInterpolateMethod.Nearest);
                    img4.InterpolativeResize(400, 600, PixelInterpolateMethod.Mesh);
                    img5.InterpolativeResize(400, 600, PixelInterpolateMethod.Background);
                    img6.InterpolativeResize(400, 600, PixelInterpolateMethod.Average16);
                    img7.AdaptiveResize(400, 600);
                    img8.Resample(400, 600);
                    img9.Scale(400, 600);

                    img1.Write("C:/Users/Paola/Desktop/Snakeware.spline.png");
                    img2.Write("C:/Users/Paola/Desktop/Snakeware.bilinear.png");
                    img3.Write("C:/Users/Paola/Desktop/Snakeware.nearest.png");
                    img4.Write("C:/Users/Paola/Desktop/Snakeware.mesh.png");
                    img5.Write("C:/Users/Paola/Desktop/Snakeware.background.png");
                    img6.Write("C:/Users/Paola/Desktop/Snakeware.average16.png");
                    img.Write("C:/Users/Paola/Desktop/Snakeware.resize.png");
                    img7.Write("C:/Users/Paola/Desktop/Snakeware.adaptive.png");
                    img8.Write("C:/Users/Paola/Desktop/Snakeware.resample.png");
                    img9.Write("C:/Users/Paola/Desktop/Snakeware.scale.png");
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //transaction.Rollback();
                return false;
            }
        }
    }
}
