# Test Assignment

Hi! This project is requested by **Struckd** to demonstrate my skills in unity, though I tried different coding skills here but I did not create different **namespace** and/or  different **AssemblyDefs** because this project is relatively small in size. The icons is being used from [UXWing](https://uxwing.com/) which is free to use without attribution.


## How to use
![UIImage](https://user-images.githubusercontent.com/25811368/189644637-6efceaf3-3ac9-424b-823c-3e5e21cbe86e.PNG)

I tried to recreate the **Struckd's** Application and by the same way user can
 - Move their camera by using one finger.
 - Rotate and zoom by using two finger .
 - Move the object on X-Z by dragging the UIEditor of the object
 - the up-down button to change the Y axis of the selected object
 - the top right corner image is used to scale up and down the item
 - and the delete button on the bottom of the editor UI to delete the selected object.

## Working Approach
The main 5 Managers classes here are 

 1. **GameManager.cs**
 2. **InputManager.cs**
 3. **UIManager.cs**
 4. **SessionHandler.cs**
 5. **TouchController.cs**
 
 The GameManager is maintaining the **GameState** and has reference of the instances of **UIManager**, **SessionHandler** and **TouchController**
The **InputHanler** is detecting the touch input and firing events
The **UIManager** is the main communication from UI to other classes
The **SessionHandler** is the maintainer of current session and saving the session to a file
The **TouchController** is mainly responsible for move around the world

## Challages

There were many challenges faced while developing this application most 2 of them are
 

 1. Change of the size of **Selected Object UI**: Here the bounds of the mesh are detecting and from the bound position, it is converted to screen space using **UIRectAreaOf3DObjects** and then the screen space is converting to Canvas Space with custom written class **CanvasExtention**.
 2. The **X-Z Plane movement of Object** changes with the rotation of the  camera, I implement an initial solution in a wrong way, which is to change the delta with the rotation value of camera. Then I used the **Transform.Forward** and **Transform.Right** vectors of the camera and get the **Normalized** direction from them and then change the **X-Z Plane** of the movement.

## Other Scripts

 1. The **Filehandler.cs** is using binary format to store the values into local file system
 2. The Scriptable Objects **SessionDataSO** storing the current session data, **CurrentSelectedItem** storing the value of the current Item that is selected and **3DItemList** is storing the 3D Items that needs to place in scene.
 3. The **ItemEditorUIScript**, **ItemScaleUIScript**, **ItemYAxisMove** and **ItemDeleteButtonScript** is used to manipulate the selected item in the world and **ItemSelectUIScript** is used to manipulate the UI that is controlling the Object.
 4. The **ItemController** is used to do the transform changes on the item by **AllItemInSceneController** class.

## Other Packages used

 1. **TMPro** for crystal clear text
 2. **InputSystem** for Unity's Enhanced Touch Input
 3. **Sprite Editor** to edit PNGs
