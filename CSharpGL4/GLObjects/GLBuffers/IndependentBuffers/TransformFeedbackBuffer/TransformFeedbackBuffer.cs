﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;

namespace CSharpGL
{
    /// <summary>
    /// 位于服务器端（GPU内存）的定长数组。
    /// <para>An array at server side (GPU memory) with fixed length.</para>
    /// </summary>
    [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]
    public partial class TransformFeedbackBuffer : GLBuffer, IDisposable
    {
        /// <summary>
        /// 位于服务器端（GPU内存）的定长数组。
        /// <para>An array at server side (GPU memory) with fixed length.</para>
        /// </summary>
        /// <param name="bufferId">用glGenBuffers()得到的VBO的Id。<para>Id got from glGenBuffers();</para></param>
        /// <param name="byteLength">此buffer中的数据在内存中占用多少个字节？<para>How many bytes in this buffer?</para></param>
        internal TransformFeedbackBuffer(uint bufferId, int byteLength)
            : base(bufferId, byteLength, byteLength)
        {
            this.Target = BufferTarget.TransformFeedbackBuffer;
        }

        public override void Bind()
        {
            glBindTransformFeedback((uint)this.Target, this.BufferId);
        }

        public override void Unbind()
        {
            glBindTransformFeedback((uint)this.Target, 0);
        }

        public override IntPtr MapBuffer(MapBufferAccess access, bool bind = true)
        {
            return base.MapBuffer(access, bind);
        }

        public override IntPtr MapBufferRange(int offset, int length, MapBufferRangeAccess access, bool bind = true)
        {
            return base.MapBufferRange(offset, length, access, bind);
        }

        public override bool UnmapBuffer(bool unbind = true)
        {
            return base.UnmapBuffer(unbind);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeNames"></param>
        /// <param name="program"></param>
        /// <param name="bufferMode"></param>
        public void Capture(string[] attributeNames, ShaderProgram program, BufferMode bufferMode)
        {
            glTransformFeedbackVaryings(program.ProgramId, attributeNames.Length, attributeNames, (uint)bufferMode);
            ShaderProgram.glLinkProgram(program.ProgramId);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum BufferMode : uint
        {
            /// <summary>
            /// 
            /// </summary>
            Separate = GL.GL_SEPARATE_ATTRIBS,

            /// <summary>
            /// 
            /// </summary>
            InterLeaved = GL.GL_INTERLEAVED_ATTRIBS,
        }
    }
}
