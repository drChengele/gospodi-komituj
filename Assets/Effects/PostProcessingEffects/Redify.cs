using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kromlech.Effects {
    [Serializable]
    [PostProcess(typeof(RedifyRenderer), PostProcessEvent.AfterStack, "Custom/Redify")]
    public class Redify : PostProcessEffectSettings {
        public FloatParameter minRed = new FloatParameter { value = 0.1f };
        public FloatParameter maxRed = new FloatParameter { value = 0.25f };
        public ColorParameter input  = new ColorParameter { value = Color.red };
        public ColorParameter output = new ColorParameter { value = Color.red };
    }

    public class RedifyRenderer: PostProcessEffectRenderer<Redify> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Redify"));
            sheet.properties.SetFloat("_minRed", settings.minRed);
            sheet.properties.SetFloat("_maxRed", settings.maxRed);
            sheet.properties.SetColor("_inputColor", settings.input);
            sheet.properties.SetColor("_outputColor", settings.output);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}