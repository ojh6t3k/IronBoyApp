using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(RectTransform))]
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public RectTransform handle;

	public bool interactable = true;

	public UnityEvent OnDragStart;
	public UnityEvent OnDrag;
	public UnityEvent OnDragEnd;

	private RectTransform _rectTransform;
	private bool _drag = false;
	private Vector2 _axis = Vector2.zero;

	// Use this for initialization
	void Start ()
	{
		_rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if(interactable == false)
			return;

		_drag = true;
		OnDragStart.Invoke();
	}
	
	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if(interactable == false)
			return;

		Vector3 pos3;
		if(RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out pos3))
		{
			handle.position = pos3;

			Vector2 pos2 = handle.anchoredPosition;
			float radius = _rectTransform.rect.width / 2f;

			if(pos2.magnitude > radius)
			{
				pos2 = pos2.normalized * radius;
				handle.anchoredPosition = pos2;
			}

			_axis.x = Mathf.Clamp(pos2.x / radius, -1f, 1f);
			_axis.y = Mathf.Clamp(pos2.y / radius, -1f, 1f);
		}
		OnDrag.Invoke();
	}
	
	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		if(interactable == false)
			return;

		handle.anchoredPosition = Vector2.zero;
		_axis = Vector3.zero;
		_drag = false;
		OnDragEnd.Invoke();
	}

	public float HorizontalAxis
	{
		get
		{
			return _axis.x;
		}
	}

	public float VerticalAxis
	{
		get
		{
			return _axis.y;
		}
	}

	public Vector2 Axis
	{
		get
		{
			return _axis;
		}
	}
}
