using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

	public int value;
	public float flashTime = 0.5f;

	public GraphicDisplaySettings[] displaySettings;

	private int oldValue;
	private Text textElement;

	private void Reset()
	{
		var graphic = GetComponent<Graphic>();
		if (graphic)
		{
			displaySettings = new[]
			{
				new GraphicDisplaySettings
				{
					graphic = graphic,
					gainColor = Color.green,
					lossColor = Color.red,
					idleColor = graphic.color
				}
			};
		}
	}

	private void OnEnable()
	{
		textElement = GetComponent<Text>();
		if (!textElement)
		{
			enabled = false;
		}
	}

	private void Update()
	{
		if (value > oldValue)
		{
			UpdateValueVisual(ValueChange.Gain);
		}
		else if (value < oldValue)
		{
			UpdateValueVisual(ValueChange.Loss);
		}
	}

	private void UpdateValueVisual(ValueChange state)
	{
		StopAllCoroutines();
		StartCoroutine(ColorFlash(state));
		oldValue = value;
		textElement.text = value.ToString(CultureInfo.InvariantCulture);
	}

	private IEnumerator ColorFlash(ValueChange state)
	{
		float time = flashTime;

		while (time > 0)
		{
			SetGraphicsColors(state, time / flashTime);
			yield return null;
			time -= Time.deltaTime;
		}

		SetGraphicsColors(ValueChange.Gain, 0);
	}

	private void SetGraphicsColors(ValueChange state, float factor)
	{
		foreach (GraphicDisplaySettings settings in displaySettings.Where(o => o.graphic))
		{
			Color newColor = state == ValueChange.Gain
				? settings.gainColor
				: settings.lossColor;

			settings.graphic.color = Color.Lerp(settings.idleColor, newColor, factor);
		}
	}

	enum ValueChange
	{
		Loss,
		Gain
	}

	[Serializable]
	public struct GraphicDisplaySettings
	{
		public Graphic graphic;
		public Color lossColor;
		public Color gainColor;
		public Color idleColor;
	}
}

