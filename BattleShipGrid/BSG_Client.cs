using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BattleShipShared.Packet;

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

            public Point ToPoint()
            {
                return new Point((int)X,(int)Y);
            }
            
        }

        public enum GridState
	    {
	            BateauxPlacer,
                None,                
                PlacementPorteAvions,
                PlacementCroiseur ,
                PlacementContreTorpilleur,
                PlacementSousMarin ,
                PlacementTorpilleur,
	    }

        #region Properties

        /// <summary>
        /// Nombre de case de la grille
        /// </summary>
        private uint PGridNumber = 10;

        /// <summary>
        /// Interface public pour le nombre de case de la grille
        /// </summary>
        public uint GridNumber 
        {
            get{return PGridNumber;}
            set{
                if(value>0)
                    PGridNumber = value;
                }
        }

        /// <summary>
        /// Raccourcie pour le calcule de la largeur d'une case de la grille
        /// </summary>
        private float GridRectWidth { get { return Width / GridNumber; } }

        /// <summary>
        /// Raccourcie pour le calcule de la hauteur d'une case de la grille
        /// </summary>
        private float GridRectHeight { get { return Height / GridNumber; } }

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

        public Image ContreTorpilleur { get; set; }
        public Image SousMarin { get; set; }
        public Image Torpilleur { get; set; }


        private const uint SizePorteAvions = 5;
        private const uint SizeCroiseur = 4;
        private const uint SizeContreTorpilleur = 3;
        private const uint SizeSousMarin = 3;
        private const uint SizeTorpilleur = 2;

        private PosShips.Orientation OCurrentShip { get; set; }

        /// <summary>
        /// Strucure qui contient la position des bateaux
        /// </summary>
        public PosShips PositionBateau { get; private set; }

        public GridState EtatGrille { get; private set; }


        #endregion
        public BattleShipGrid()
        {
            InitializeComponent();
            //DoubleBuffered = true;
            EtatGrille = GridState.None;
            OCurrentShip = PosShips.Orientation.Horizontale;
            PositionBateau = new PosShips();
            //this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint)

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }
        /// <summary>
        /// Action lors du click sur la grille
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            //Sauvegarde la case dans laquelle est la souris lors du click
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Right)//Right click pour changé l'orientation
            {
                if (OCurrentShip == PosShips.Orientation.Horizontale)
                    OCurrentShip = PosShips.Orientation.Verticale;
                else
                    OCurrentShip = PosShips.Orientation.Horizontale;
            }
            else
            {
                switch (EtatGrille)
                {
                    case GridState.BateauxPlacer:
                        break;
                    case GridState.None:
                        //EtatGrille = GridState.PlacementPorteAvions;
                        break;
                    case GridState.PlacementPorteAvions:
                        PositionBateau.PPorteAvion = GetGridCoordOfMouse().ToPoint();
                        PositionBateau.OPorteAvion = OCurrentShip;
                        EtatGrille = GridState.PlacementCroiseur;
                        break;
                    case GridState.PlacementCroiseur:
                        PositionBateau.PCroiseur = GetGridCoordOfMouse().ToPoint();
                        PositionBateau.OCroiseur = OCurrentShip;
                        EtatGrille = GridState.PlacementContreTorpilleur;
                        break;
                    case GridState.PlacementContreTorpilleur:
                        PositionBateau.PContreTorpilleur = GetGridCoordOfMouse().ToPoint();
                        PositionBateau.OContreTorpilleur = OCurrentShip;
                        EtatGrille = GridState.PlacementSousMarin;
                        break;
                    case GridState.PlacementSousMarin:
                        PositionBateau.PSousMarin = GetGridCoordOfMouse().ToPoint();
                        PositionBateau.OSousMarin = OCurrentShip;
                        EtatGrille = GridState.PlacementTorpilleur;
                        break;
                    case GridState.PlacementTorpilleur:
                        PositionBateau.PTorpilleur = GetGridCoordOfMouse().ToPoint();
                        PositionBateau.OTorpilleur = OCurrentShip;
                        EtatGrille = GridState.BateauxPlacer;
                        break;
                    default:
                        break;
                }
                Refresh();
                /*if (EtatGrille < GridState.PlacementTorpilleur)
                    EtatGrille++;*/
                FPoint coords = GetGridCoordOfMouse();
                //DrawSelection(coords);
            }
            

            //MessageBox.Show(PGridColor.ToString() + coords.X.ToString() + " " + coords.Y.ToString());
        }

        public void DebutPlacerBateaux()
        {
            if(EtatGrille==GridState.None)
            EtatGrille = GridState.PlacementPorteAvions;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(EtatGrille != GridState.BateauxPlacer && EtatGrille != GridState.None )
            Refresh();
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
            //Limite la position au nombre max de la grille
            if (mouse.X >= GridNumber)
                mouse.X = GridNumber - 1;
            if (mouse.Y >= GridNumber)
                mouse.Y = GridNumber - 1;
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

            switch (EtatGrille)
            {
                case GridState.BateauxPlacer:
                    goto case GridState.PlacementTorpilleur;
                case GridState.PlacementTorpilleur:
                    if (Torpilleur != null)
                        DrawSingleShip(Torpilleur, GridState.PlacementTorpilleur);
                    goto case GridState.PlacementSousMarin;
                case GridState.PlacementSousMarin:
                    if (SousMarin != null)
                        DrawSingleShip(SousMarin, GridState.PlacementSousMarin);
                    goto case GridState.PlacementContreTorpilleur;
                case GridState.PlacementContreTorpilleur:
                    if (ContreTorpilleur != null)
                        DrawSingleShip(ContreTorpilleur, GridState.PlacementContreTorpilleur);
                    goto case GridState.PlacementCroiseur;
                case GridState.PlacementCroiseur:
                    if (Croiseur != null)
                        DrawSingleShip(Croiseur, GridState.PlacementCroiseur);
                    goto case GridState.PlacementPorteAvions;
                case GridState.PlacementPorteAvions:
                    if (PorteAvions != null)
                        DrawSingleShip(PorteAvions, GridState.PlacementPorteAvions);
                    goto default;
                default:
                    break;

            }
            /*switch (EtatGrille)
            {
                case GridState.BateauxPlacer:
                    
                case GridState.PlacementPorteAvions:
                    if (PorteAvions != null)
                        DrawImage(PorteAvions, 2 * GridRectWidth, 2 * GridRectHeight, 5 * GridRectWidth, 1 * GridRectHeight);
                case GridState.PlacementCroiseur:
                    break;
                case GridState.PlacementSousMarin:
                    break;
                case GridState.PlacementTorpilleur:
                    break;
                default:
                    break;
            }*/
            /*if(PorteAvions!= null)
                DrawImage(PorteAvions,2*GridRectWidth,2*GridRectHeight,5*GridRectWidth,1*GridRectHeight);*/
        }

        private void DrawSingleShip(Image img, GridState ImgState)
        {
            
            if (EtatGrille == ImgState) // si dans le même état que l'image dessine un preview
            {
                FPoint pos = GetGridCoordOfMouse();
                if (OCurrentShip == PosShips.Orientation.Verticale)
                {
                    Image carry = (Image)img.Clone();
                    carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    switch (ImgState)
                    {
                        case GridState.PlacementPorteAvions:
                            DrawImage(carry, pos.X * GridRectWidth, pos.Y * GridRectHeight, 1 * GridRectWidth, SizePorteAvions * GridRectHeight);
                            break;
                        case GridState.PlacementCroiseur:
                            DrawImage(carry, pos.X * GridRectWidth, pos.Y * GridRectHeight, 1 * GridRectWidth, SizeCroiseur * GridRectHeight);
                            break;
                        case GridState.PlacementContreTorpilleur:
                            DrawImage(carry, pos.X * GridRectWidth, pos.Y * GridRectHeight, 1 * GridRectWidth, SizeContreTorpilleur * GridRectHeight);
                            break;
                        case GridState.PlacementSousMarin:
                            DrawImage(carry, pos.X * GridRectWidth, pos.Y * GridRectHeight, 1 * GridRectWidth, SizeSousMarin * GridRectHeight);
                            break;
                        case GridState.PlacementTorpilleur:
                            DrawImage(carry, pos.X * GridRectWidth, pos.Y * GridRectHeight, 1 * GridRectWidth, SizeTorpilleur * GridRectHeight);
                            break;
                        default:
                            break;
                    }
                    
                }
                else
                {
                    switch (ImgState)
                    {
                        case GridState.PlacementPorteAvions:
                            DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizePorteAvions * GridRectWidth, 1 * GridRectHeight);
                            break;
                        case GridState.PlacementCroiseur:
                            DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizeCroiseur * GridRectWidth, 1 * GridRectHeight);
                            break;
                        case GridState.PlacementContreTorpilleur:
                            DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizeContreTorpilleur * GridRectWidth, 1 * GridRectHeight);
                            break;
                        case GridState.PlacementSousMarin:
                            DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizeSousMarin * GridRectWidth, 1 * GridRectHeight);
                            break;
                        case GridState.PlacementTorpilleur:
                            DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizeTorpilleur * GridRectWidth, 1 * GridRectHeight);
                            break;
                        default:
                            break;
                    }
                    DrawImage(img, pos.X * GridRectWidth, pos.Y * GridRectHeight, SizeTorpilleur * GridRectWidth, 1 * GridRectHeight);
                }

            }
            else // si pas dans le même état
            {
                switch (ImgState)
                {
                    case GridState.PlacementPorteAvions:
                        if (PositionBateau.OPorteAvion == PosShips.Orientation.Verticale)
                        {
                            Image carry = (Image)img.Clone();
                            carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            DrawImage(carry, PositionBateau.PPorteAvion.X * GridRectWidth, PositionBateau.PPorteAvion.Y * GridRectHeight, 1 * GridRectWidth, SizePorteAvions * GridRectHeight);
                        }
                        else
                        {
                            DrawImage(img, PositionBateau.PPorteAvion.X * GridRectWidth, PositionBateau.PPorteAvion.Y * GridRectHeight, SizePorteAvions * GridRectWidth, 1 * GridRectHeight);
                        }
                        break;
                    case GridState.PlacementCroiseur:
                        if (PositionBateau.OCroiseur == PosShips.Orientation.Verticale)
                        {
                            Image carry = (Image)img.Clone();
                            carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            DrawImage(carry, PositionBateau.PCroiseur.X * GridRectWidth, PositionBateau.PCroiseur.Y * GridRectHeight, 1 * GridRectWidth, SizeCroiseur * GridRectHeight);
                        }
                        else
                        {
                            DrawImage(img, PositionBateau.PCroiseur.X * GridRectWidth, PositionBateau.PCroiseur.Y * GridRectHeight, SizeCroiseur * GridRectWidth, 1 * GridRectHeight);
                        }
                        break;
                    case GridState.PlacementContreTorpilleur:
                        if (PositionBateau.OContreTorpilleur == PosShips.Orientation.Verticale)
                        {
                            Image carry = (Image)img.Clone();
                            carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            DrawImage(carry, PositionBateau.PContreTorpilleur.X * GridRectWidth, PositionBateau.PContreTorpilleur.Y * GridRectHeight, 1 * GridRectWidth, SizeContreTorpilleur * GridRectHeight);
                        }
                        else
                        {
                            DrawImage(img, PositionBateau.PContreTorpilleur.X * GridRectWidth, PositionBateau.PContreTorpilleur.Y * GridRectHeight, SizeContreTorpilleur * GridRectWidth, 1 * GridRectHeight);
                        }
                        break;
                    case GridState.PlacementSousMarin:
                        if (PositionBateau.OSousMarin == PosShips.Orientation.Verticale)
                        {
                            Image carry = (Image)img.Clone();
                            carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            DrawImage(carry, PositionBateau.PSousMarin.X * GridRectWidth, PositionBateau.PSousMarin.Y * GridRectHeight, 1 * GridRectWidth, SizeSousMarin * GridRectHeight);
                        }
                        else
                        {
                            DrawImage(img, PositionBateau.PSousMarin.X * GridRectWidth, PositionBateau.PSousMarin.Y * GridRectHeight, SizeSousMarin * GridRectWidth, 1 * GridRectHeight);
                        }
                        break;
                    case GridState.PlacementTorpilleur:
                        if (PositionBateau.OTorpilleur == PosShips.Orientation.Verticale)
                        {
                            Image carry = (Image)img.Clone();
                            carry.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            DrawImage(carry, PositionBateau.PTorpilleur.X * GridRectWidth, PositionBateau.PTorpilleur.Y * GridRectHeight, 1 * GridRectWidth, SizeTorpilleur * GridRectHeight);
                        }
                        else
                        {
                            DrawImage(img, PositionBateau.PTorpilleur.X * GridRectWidth, PositionBateau.PTorpilleur.Y * GridRectHeight, SizeTorpilleur * GridRectWidth, 1 * GridRectHeight);
                        }
                        break;
                    default:
                        break;
                }
            }
                //DrawImage(Torpilleur, PositionBateau.PTorpilleur.X * GridRectWidth, PositionBateau.PTorpilleur.Y * GridRectHeight, 5 * GridRectWidth, 1 * GridRectHeight);

            
        }

        private void DrawImage(Image img, float x, float y, float width, float height)
        {
            Graphics graph = this.CreateGraphics();
            //BufferedGraphicsContext graph = BufferedGraphicsManager.Current;
            graph.DrawImage(img,x,y,width,height);
            //MessageBox.Show(x.)
            
        }
        /// <summary>
        /// Dessine le rectangle sous la coordonné
        /// </summary>
        /// <param name="coords"></param>
        private void DrawSelection(FPoint coords)
        {
            if(coords.X<9 && coords.Y<9)
                DrawRect(BorderOfSelection, InteriorOfSelection, coords.X * GridRectWidth, coords.Y * GridRectHeight, GridRectWidth, GridRectHeight);
            else if (coords.X >= 9 && coords.Y >= 9)
                DrawRect(BorderOfSelection, InteriorOfSelection, coords.X * GridRectWidth, coords.Y * GridRectHeight,Width - coords.X * GridRectWidth, Height - coords.Y* GridRectHeight);
            else if(coords.X>=9)
                DrawRect(BorderOfSelection, InteriorOfSelection, coords.X * GridRectWidth, coords.Y * GridRectHeight, Width - coords.X * GridRectWidth, GridRectHeight);
            else if(coords.Y>=9)
                DrawRect(BorderOfSelection, InteriorOfSelection, coords.X * GridRectWidth, coords.Y * GridRectHeight, GridRectWidth, Height - coords.Y * GridRectHeight);



        }

        /// <summary>
        /// Dessine la grille
        /// </summary>
        private void DrawGrid()
        {
            for (int i = 1; i < GridNumber; i++)
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
