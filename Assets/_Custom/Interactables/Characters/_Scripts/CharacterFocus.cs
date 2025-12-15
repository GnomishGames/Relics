using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterFocus : MonoBehaviour
{
	[Header("Role")]
	public bool isPlayer = false;

	[Header("State")]
	public Interactable currentFocus; // for players: what they are focusing; for NPCs: optional
    public float distanceToFocus;

	[Header("Player refs")]
	public Camera cam;
	public Inventory inventory;

	[Header("Tracking")]
	public List<CharacterFocus> charactersTargetingMe = new List<CharacterFocus>();

	[Header("Perception")]
	public FieldOfView fieldOfView;

	// cached reference to this object's Interactable (owner)
	private Interactable selfInteractable;

	void Awake()
	{
		selfInteractable = GetComponent<Interactable>();
		if (fieldOfView == null)
			fieldOfView = GetComponent<FieldOfView>();

		if (isPlayer)
		{
			if (cam == null)
				cam = Camera.main;
			if (inventory == null)
				inventory = GetComponentInChildren<Inventory>();
		}
	}

	void Update()
	{
		if (isPlayer)
		{
			HandlePlayerInput();
            fieldOfView.FindVisibleTargets();
		}
		else
		{
			PruneStaleTargeters();
		}

		if (currentFocus != null)
		{
			distanceToFocus = Vector3.Distance(currentFocus.transform.position, transform.position);

			// Drop focus if out of view radius or not in visible targets
			if (fieldOfView != null)
			{
				float viewRadius = fieldOfView.viewRadius;
				bool tooFar = distanceToFocus > viewRadius;
				bool notVisibleList = !fieldOfView.visibleTargets.Contains(currentFocus);
				if (tooFar || notVisibleList)
				{
					RemoveFocus();
				}
			}
		}
		else
		{
			distanceToFocus = 0f;
		}
    }

	private void HandlePlayerInput()
	{
		if (cam == null) return;

		//don't click through the UI
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100f))
			{
				Character character = hit.collider.GetComponent<Character>();
				Item item = hit.collider.GetComponent<Item>();
				Container container = hit.collider.GetComponent<Container>();

				if (character != null)
				{
					SetCharacterFocus(character);
				}
				else if (item != null)
				{
					SetItemFocus(item);
				}
				else if (container != null)
				{
					SetContainerFocus(container);
				}
			}
		}
	}

	private void PruneStaleTargeters()
	{
		if (selfInteractable == null)
			selfInteractable = GetComponent<Interactable>();

		for (int i = charactersTargetingMe.Count - 1; i >= 0; i--)
		{
			var characterFocus = charactersTargetingMe[i];
			if (characterFocus == null)
			{
				charactersTargetingMe.RemoveAt(i);
				continue;
			}

			// Remove if that character is no longer focusing on this interactable
			if (characterFocus.currentFocus != selfInteractable)
			{
				charactersTargetingMe.RemoveAt(i);
			}
		}
	}

	public void SetCharacterFocus(Character character)
	{
		if (character == null) return;
		if (character != currentFocus) //new focus
		{
			if (currentFocus != null)
				currentFocus.onDeFocus();

			// Tell the target we are focusing it
			var targetCF = character.GetComponent<CharacterFocus>();
			if (targetCF != null)
			{
				targetCF.OnFocused(this.transform);
			}

			currentFocus = character;
		}


		character.OnFocused(transform); // Notify the character
	}

	private void SetItemFocus(Item item)
	{
		if (item == null) return;
		if (item != currentFocus)
		{
			if (currentFocus != null)
				currentFocus.onDeFocus();
			currentFocus = item;
		}

		item.OnFocused(transform);

		if (inventory != null)
		{
			inventory.PickupItem(item);
		}
	}

	private void SetContainerFocus(Container container)
	{
		if (container == null) return;
		if (container != currentFocus)
		{
			if (currentFocus != null)
				currentFocus.onDeFocus();
			currentFocus = container;
		}
		container.OnFocused(transform);
	}

	public void RemoveFocus()
	{
		if (currentFocus != null)
			currentFocus.onDeFocus();
		currentFocus = null;
	}

	public void OnFocused(Transform source)
	{
		var cf = source != null ? source.GetComponent<CharacterFocus>() : null;
		if (cf != null && !charactersTargetingMe.Contains(cf))
		{
			charactersTargetingMe.Add(cf);
		}
	}

	public void OnDeFocus(Transform source)
	{
		var characterFocus = source != null ? source.GetComponent<CharacterFocus>() : null;
		if (characterFocus != null)
		{
			charactersTargetingMe.Remove(characterFocus);
		}
	}
}
