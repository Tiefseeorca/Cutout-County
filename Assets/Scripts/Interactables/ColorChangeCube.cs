using UnityEngine;

public class ColorChangeCube : Interactable {
    private Renderer _cubeRenderer;

    void Start(){
        _cubeRenderer = GetComponent<Renderer>();
    }
    
    public override void TryInteract(){
        Color randomColor = Random.ColorHSV();
        _cubeRenderer.material.color = randomColor;
    }
}