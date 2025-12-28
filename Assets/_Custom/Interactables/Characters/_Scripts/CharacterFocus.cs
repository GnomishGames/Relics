using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterFocus : MonoBehaviour
{
	[Header("Role")]
	public bool isPlayer = false;

	[Header("State")]
	public Interactable target; // for players: what they are focusing; for NPCs: optional
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

	//target panel
	public TargetPanel targetPanel;

	// create event for when this character has a focus
	public event Action<string, float, float> OnTargetFocused;

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
		}
		else
		{
			PruneStaleTargeters();
		}

		if (target != null)
		{
			distanceToFocus = Vector3.Distance(target.transform.position, transform.position);

			// Drop focus if out of view radius or not in visible targets
			if (fieldOfView != null)
			{
				float viewRadius = fieldOfView.viewRadius;
				bool tooFar = distanceToFocus > viewRadius;
				bool notVisibleList = !fieldOfView.visibleTargets.Contains(target);
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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			RemoveFocus();
		}

		//update target panel
		if (targetPanel != null) //npcs may not have a target panel
		{
			if (target != null)
				targetPanel.gameObject.SetActive(true);
				
				
			else
				targetPanel.gameObject.SetActive(false);
		}
	}

	void OnNewFocus()
	{
		if (targetPanel != null && target != null)
		{
			// Activate panel and update with new target
			targetPanel.gameObject.SetActive(true);
			targetPanel.SetNewTarget(target.GetComponent<CharacterStats>());
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
				CharacterStats character = hit.collider.GetComponent<CharacterStats>();
				Item item = hit.collider.GetComponent<Item>();
				Container container = hit.collider.GetComponent<Container>();

				if (character != null)
				{
					SetCharacterFocus(character);
					OnTargetFocused?.Invoke(character.name, character.currentHitPoints, character.currentStamina);
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
			if (characterFocus.target != selfInteractable)
			{
				charactersTargetingMe.RemoveAt(i);
			}
		}
	}

	public void SetCharacterFocus(CharacterStats character)
	{
		if (character == null) return;
		if (character != target) //new focus
		{
			if (target != null)
				target.onDeFocus();

			// Tell the target we are focusing it
			var targetCF = character.GetComponent<CharacterFocus>();
			if (targetCF != null)
			{
				targetCF.OnFocused(this.transform);
			}

			target = character;
			OnNewFocus();
		}

		character.OnFocused(transform); // Notify the character
	}

	private void SetItemFocus(Item item)
	{
		if (item == null) return;
		if (item != target)
		{
			if (target != null)
				target.onDeFocus();
			target = item;
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
		if (container != target)
		{
			if (target != null)
				target.onDeFocus();
			target = container;
		}
		container.OnFocused(transform);
	}

	public void RemoveFocus()
	{
		if (target != null)
			target.onDeFocus();
		target = null;
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
