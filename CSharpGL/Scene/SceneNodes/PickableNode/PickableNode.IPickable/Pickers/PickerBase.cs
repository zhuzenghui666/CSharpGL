﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// Get picked geometry.
    /// </summary>
    abstract class PickerBase
    {
        /// <summary>
        /// Get picked geometry.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="PositionBuffer"></param>
        /// <param name="drawCommand"></param>
        public PickerBase(PickableNode node, VertexBuffer positionBuffer, IDrawCommand drawCommand)
        {
            this.Node = node;
            this.PositionBuffer = positionBuffer;
            this.DrawCommand = drawCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="stageVertexId">The last vertex's id that constructs the picked primitive.
        /// <para>This id is in scene's all <see cref="IPickable"/>s' order.</para></param>
        /// <param name="baseId">Index of first vertex of the buffer that The geometry belongs to.
        /// <para>This id is in scene's all <see cref="IPickable"/>s' order.</para></param>
        /// <returns></returns>
        public abstract PickedGeometry GetPickedGeometry(PickingEventArgs arg, uint stageVertexId, uint baseId);

        /// <summary>
        /// 
        /// </summary>
        public PickableNode Node { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public VertexBuffer PositionBuffer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDrawCommand DrawCommand { get; private set; }

        protected vec3[] FillPickedGeometrysPosition(uint firstIndex, int indexCount)
        {
            var positionIndexes = new uint[indexCount];
            for (uint i = 0; i < indexCount; i++)
            {
                positionIndexes[i] = firstIndex + i;
            }

            return FillPickedGeometrysPosition(positionIndexes);
        }

        protected vec3[] FillPickedGeometrysPosition(uint[] positionIndexes)
        {
            VertexBuffer[] buffers = this.Node.PickingRenderUnit.PositionBuffer;
            var workItems = buffers.GetWorkItems(positionIndexes);
            var positions = new List<vec3>();
            foreach (var item in workItems)
            {
                VertexBuffer buffer = buffers[item.Key];
                IntPtr pointer = buffer.MapBuffer(MapBufferAccess.ReadOnly);
                unsafe
                {
                    var array = (vec3*)pointer.ToPointer();
                    foreach (var tuple in item)
                    {
                        positions.Add(array[tuple.indexInBuffer]);
                    }
                }
                buffer.UnmapBuffer();
            }

            return positions.ToArray();
        }


    }
}
