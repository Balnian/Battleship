using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipGrid
{
    public partial class BattleShipGrid : UserControl
    {

        class FPoint
        {
            public float X;
            public float Y;
            public FPoint(float x, float y)
            {
                X = x;
                Y = y;
            }
            public FPoint(Point p)
            {
                X=p.X;
                Y=p.Y;
            }
            
        }

        #region Properties
        /// <summary>
        /// Raccourcie pour le calcule de la largeur d'une case de la grille
        /// </summary>
        private float GridRectWidth { get { return Width / 10; } }

        /// <summary>
        /// Raccourcie pour le calcule de la hauteur d'une case de la grille
        /// </summary>
        private float GridRectHeight { get { return Height / 10; } }

        /// <summary>
        /// Couleur de la grille
        /// </summary>
        private Color PGridColor = Color.Black;

        /// <summary>
        /// Interface public pour la couleur de la grille
        /// </summary>
        public Color GridColor
        {
            get { return PGridColor; }
            set { PGridColor = value; }
        }

        /// <summary>
        /// Couleur de la bordure de la sélection
        /// </summary>
        private Color PBorderOfSelection = Color.Transparent;

        /// <summary>
        /// Interface public pour la couleur de la bordure de la sélection
        /// </summary>
        public Color BorderOfSelection
        {
            get { return PBorderOfSelection; }
            set { PBorderOfSelection = value; }
        }

        /// <summary>
        /// Couleur de l'intérieur de la sélection
        /// </summary>
        private Color PInteriorOfSelection = Color.Red;

        /// <summary>
        /// Interface public pour la couleur de l'intérieur de la sélection
        /// </summary>
        public Color InteriorOfSelection
        {
            get { return PInteriorOfSelection; }
            set { PInteriorOfSelection = value; }
        }

        public Image PorteAvions { get; set; }
        public Image Croiseur { get; set; }
        public Image SousMarin { get; set; }
        public Image Torpilleur { get; set; }

        #endregion
        public BattleShipGrid()
        {
            InitializeComponent();

        }
       

        /// <summary>
        /// Action lors du click sur la grille
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            Refresh();
            FPoint coords = GetGridCoordOfMouse();
            DrawSelection(coords);

            //MessageBox.Show(PGridColor.ToString() + coords.X.ToString() + " " + coords.Y.ToString());
        }

        /// <summary>
        /// retourne les coordonnées dans la grille où se trouve la souris
        /// </summary>
        /// <returns>Coordonnées de la grille</returns>
        private FPoint GetGridCoordOfMouse()
        {
            FPoint mouse = new FPoint(this.PointToClient(Cursor.Position));
            mouse.X = (float)Math.Floor(mouse.X / GridRectWidth);
            mouse.Y = (float)Math.Floor(mouse.Y / GridRectHeight);
            //MessageBox.Show(mouse.X + " " + mouse.Y);
            return mouse;
            
        }

        /// <summary>
        /// Surcharge de la méthode on paint pour y dessiner la grille
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(pe);
            //Dessine la Grille
            DrawGrid();
            FPoint coords = GetGridCoordOfMouse();
            DrawShips();

           // DrawRect(Color.Aquamarine, Color.Chocolate, coords.X * GridRectWidth, coords.Y * GridRectHeight, GridRectWidth, GridRectHeight);
        }

        #region Draw

        private void DrawShips()
        {
            if(PorteAvions!= null)
                DrawImage(PorteAvions,2*GridRectWidth,2*GridRectHeight,5*GridRectWidth,1*GridRectHeight);
        }

        private void DrawImage(Image img, float x, float y, float width, float height)
        {
            Graphics graph = this.CreateGraphics();
            graph.DrawImage(img,x,y,width,height);
            //MessageBox.Show(x.)
            
        }
        /// <summary>
        /// Dessine le rectangle sous la coordonné
        /// </summary>
        /// <param name="coords"></param>
        private void DrawSelection(FPoint coords)
        {
            
            DrawRect(BorderOfSelection, InteriorOfSelection, coords.X * GridRectWidth, coords.Y * GridRectHeight, GridRectWidth, GridRectHeight);
        }

        /// <summary>
        /// Dessine la grille
        /// </summary>
        private void DrawGrid()
        {
            for (int i = 1; i < 10; i++)
            {
                //ligne vertical
                DrawLine(PGridColor, GridRectWidth * i, 0, GridRectWidth * i, this.Size.Height);

                //Ligne Horizontale
                DrawLine(PGridColor, 0, GridRectHeight * i, this.Size.Width, GridRectHeight * i);
            }
        }

        /// <summary>
        /// Dessine une ligne
        /// </summary>
        /// <param name="couleur">Couleur de la ligne</param>
        /// <param name="StartX">Début en X de la ligne</param>
        /// <param name="StartY">Début en Y de la  ligne</param>
        /// <param name="EndX">Fin en X de la ligne</param>
        /// <param name="EndY">Fin en Y de la ligne</param>
        private void DrawLine(Color couleur, float StartX, float StartY, float EndX, float EndY)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(couleur);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawLine(myPen, StartX, StartY, EndX, EndY);
            myPen.Dispose();
            formGraphics.Dispose();
        }

        /// <summary>
        /// Dessine un rectangle
        /// </summary>
        /// <param name="BorderColor">Couleur de la bordure</param>
        /// <param name="FillColor">Couleur de l'intérieur</param>
        /// <param name="x">Origine en X du rectangle</param>
        /// <param name="y">Origine en Y du rectangle</param>
        /// <param name="width">Largeur du rectangle</param>
        /// <param name="height">Hauteur du rectangle</param>
        private void DrawRect(Color BorderColor, Color FillColor, float x, float y, float width, float height)
        {

            Pen myPen;
            myPen = new System.Drawing.Pen(BorderColor);
            Brush brush = new SolidBrush(FillColor);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(brush, x, y, width, height);
            formGraphics.DrawRectangle(myPen, x, y, width, height);
            myPen.Dispose();
            brush.Dispose();
            formGraphics.Dispose();

        }
        #endregion
    }
}
