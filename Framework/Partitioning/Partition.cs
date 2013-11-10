using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sunfish.Views.Partitioning
{
	public class Partition
	{

		public PartitionCell[,] Cells;

		public int Rows { get; private set; }

		public int Cols { get; private set; }

		public int CellSize { get; private set; }

		public Partition (int screenWidth, int screenHeight, int cellSize)
		{
			CellSize = cellSize;
			Rows = (int) Math.Ceiling( (float)screenHeight / (float)cellSize );
			Cols = (int) Math.Ceiling( (float)screenWidth / (float)cellSize );
			Cells = new PartitionCell[Rows, Cols];
			for (int row=0; row < Rows; row++) {
				for (int col=0; col < Cols; col++) {
					Cells [row, col] = new PartitionCell ();
				}
			}
		}

		public void UpdatePartitionCells(View view, List<PartitionCell> cells)
		{

			// Potential improvement: get the firstRow, lastRow, firstCol, and lastCol from
			// cells, compare to the newly computed values, and only update the cells collection
			// if there is a difference

			foreach (PartitionCell cell in cells) {
				cell.RemoveView (view);
			}
			cells.Clear ();

			Rectangle boundingBox = view.GetBoundingBox ();
			int firstRow = GetCellRow (boundingBox.Y);
			int lastRow = GetCellRow (boundingBox.Bottom);
			int firstCol = GetCellCol (boundingBox.X);
			int lastCol = GetCellCol (boundingBox.Right);
			for (int row=firstRow; row <= lastRow; row++) {
				for (int col=firstCol; col <= lastCol; col++) {
					PartitionCell cell = Cells [row, col];
					cell.AddView (view);
					cells.Add (cell);
				}
			}

		}

		public List<View> GetCollidedViews(View view)
		{

			Rectangle viewsBoundingBox = view.GetBoundingBox ();
			Dictionary<View, bool> collidedViews = new Dictionary<View, bool> ();

			foreach (PartitionCell cell in view.CollisionCells) {
				foreach (View otherView in cell.Views) {
					if (otherView.Visible && !collidedViews.ContainsKey(otherView) && otherView != view) {
						Rectangle otherViewsBoundingBox = otherView.GetBoundingBox ();
						if (otherViewsBoundingBox.Intersects (viewsBoundingBox)) {
							collidedViews.Add (otherView, true);
						}
					}
				}
			}

			List<View> result = new List<View> ();
			foreach (View collidedView in collidedViews.Keys) {
				result.Add (collidedView);
			}

			return result;

		}

		public static Vector2 GetMinimumTranslationVector(Rectangle viewBoundingBox, Rectangle otherViewBoundingBox)
		{
			Vector2 amin = new Vector2(viewBoundingBox.Left, viewBoundingBox.Top);
			Vector2 amax = new Vector2(viewBoundingBox.Right, viewBoundingBox.Bottom); 
			Vector2 bmin = new Vector2(otherViewBoundingBox.Left, otherViewBoundingBox.Top);
			Vector2 bmax = new Vector2(otherViewBoundingBox.Right, otherViewBoundingBox.Bottom); 

			Vector2 mtd = new Vector2();

			float left = (bmin.X - amax.X);
			float right = (bmax.X - amin.X);
			float top = (bmin.Y - amax.Y);
			float bottom = (bmax.Y - amin.Y);

			// box dont intersect   
			if (left > 0 || right < 0) throw new Exception("no intersection");
			if (top > 0 || bottom < 0) throw new Exception("no intersection");

			// box intersect. work out the mtd on both x and y axes.
			if (Math.Abs(left) < right)
				mtd.X = left;
			else
				mtd.X = right;

			if (Math.Abs(top) < bottom)
				mtd.Y = top;
			else
				mtd.Y = bottom;

			// 0 the axis with the largest mtd value.
			if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
				mtd.Y = 0;
			else
				mtd.X = 0; 
			return mtd;
		}

		private int GetCellRow(int y)
		{
			return MathHelper.Clamp ((int)Math.Floor ((float)y / (float)CellSize), 0, Rows - 1);
		}

		private int GetCellCol(int x)
		{
			return MathHelper.Clamp ((int)Math.Floor ((float)x / (float)CellSize), 0, Cols - 1);
		}

	}
}

