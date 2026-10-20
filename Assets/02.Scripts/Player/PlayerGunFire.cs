using UnityEngine;

//mouse left button -> fire at center
public class PlayerGunFire : MonoBehaviour
{
    private float _range = 100f;
    private float _damage = 10f;

    private void Update()
    {
        //1. mouse left click
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //2. create lay, set fire position and direction
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        //3. create ray hit target variable
        RaycastHit hitInfo = new RaycastHit();

        //4. fire ray and diplay effect when hit
        bool isHit = Physics.Raycast(ray, out hitInfo, _range);
        if (isHit)
        {
            GameLogger.Log($"Hit {hitInfo.collider.name} by bullet, damage: {_damage}");
            //ToDo: create effect
        }
    }
}