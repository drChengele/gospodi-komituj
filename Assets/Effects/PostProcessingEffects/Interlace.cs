using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kromlech.Effects {
    [Serializable]
    [PostProcess(typeof(InterlaceRenderer), PostProcessEvent.AfterStack, "Custom/Interlace")]
    public class Interlace : PostProcessEffectSettings {
        [Range(16, 1024)]
        public FloatParameter lineCount = new FloatParameter { value = 128 };
        [Range(0f, 1f)]
        public FloatParameter multiplier = new FloatParameter { value = 0.5f };
    }

    public class InterlaceRenderer : PostProcessEffectRenderer<Interlace> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Interlace"));
            sheet.properties.SetFloat("_lineCount", settings.lineCount);
            sheet.properties.SetFloat("_multiplier", settings.multiplier);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}