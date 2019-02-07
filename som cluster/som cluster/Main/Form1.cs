using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using som_cluster.Bean;
using som_cluster.Main;

namespace som_cluster
{
   
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        
        
        public Form1()
        {
            //Part #1 초기화 부분 
            InitializeComponent();
            String imagePath = "C:\\Users\\KYJ\\Desktop\\WorkSpace\\som cluster\\som cluster\\Image\\test.jpg";
            bitmap = new Bitmap(Image.FromFile(imagePath));

            //Part #2 전처리 부분
            Pretreatment prtment = new Pretreatment(bitmap);
            prtment.Gray();
            bitmap = prtment.getBitmap();

            
            //Part #3 A.I 기법 부분.
            AI aI = new AI(new Bitmap(bitmap));
            aI.Kmeans();
            Lookup lookup = new Lookup(aI.getBitmap());
            lookup.Show();

            pictureBox1.Image = prtment.getBitmap();
            pictureBox2.Image = aI.getBitmap();
            
        }
    }
}
