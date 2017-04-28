# Pedestrian-Behavior-Toolkit

## Unity Version: 5.5

### Steps to setup the project locally:
```   
1. Clone the repository to a location of your choice.
2. Open the project using the OPEN a NEW project option from Unity.
3. Go to Assets/_Scenes/Pedestrain Behavior Test to open the first scene
```

This toolkit provides the code base for generating virtual Humanoid model based Pedestrians in a 3D virtual environment.
With almost close to infinite possible scenarios, there is no simulation application that is exhaustive of all of such scenarios. Since, Humans lie at the center of every real life scenario, there are no limited number of behvioral patterns that can be associated with the behavior models of humans in a simulated environment. Hence, there is always scope for improvement, adding new behavior models or incremental development of such models as the requirement asks for.

The major components used to develop a modular toolkit that allows to add infinite number of behavior models on top of Humanoid based 3D Pedestrian models were:

1. Playmaker - In Editor Unity extension that allows to create self looping state machines. Perfect tool for deploying various human  behavioral models.

2. Simple Waypoint System - SWS works in conjunction with Playmaker state machines to provide the waypoint infrastructure for the Pedestrian behavior models. Waypoint system is responsible to give AI to the virtual Pedestrians and to give them a fixed path to follow.

3. Data parser and a path generating tool to modify, attach or deattach the above mentioned components on a Pedestrian model.

Note : Since, Playmaker and Simple Waypoint System are external third party licensed tools and libraries used in development they cannot be made part of an open source application code. The repository consists of all the plug points necessary to run and furthur develop this application, given that the components (Playmaker and SWS) are attached/imported locally.

### Development and Interaction with the toolkit

1. The sample scene consists of the following components:
  - Scene Camera
  - A 3D plane object for the Pedestrians to walk on
  - Canvas and UI button for sending events to generate/Instantiate Pedestrians
  - Ped Manager object which serves as the parent object to store all the generated Peds in the scene
  - Waypoint Manager object that serves as the parent object to store the respective waypoint based paths for each Pedestrian
  - Scene Manager gameObject to run the scripts responsible for parsing external dataset and generate waypoint based paths from them.
    This is coupled with Ped Behavior templates and waypoint paths to generate Pedestrians for simulating different real life situtions.
  
2. Pool of sample Pedestrian Behavior Models is located at Assets/_FSMTemplates. 
 
3. Pool of sample input position data set for generating waypoint based paths for Pedestrain. All the Ped behavior models and waypoint      paths are modeled based on verified surveys by US road journals and TransSafety.

4. Following are the steps to integrate Playmaker and SWS with the current implementation of the project:
  - Import/Get Playmaker from Unity Asset Store
  
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509092/4bb3403c-2b6b-11e7-996f-b8307ac51726.png)
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509221/32ee0e96-2b6c-11e7-8f08-57709bc00341.png)    
    
  - After installing Playmaker please import and install Simple Waypoint System(SWS). SWS integration with Playmaker forms the core of       the Ped Behavior models.
  
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509399/8d98729a-2b6d-11e7-82c7-b0896cb806b1.png)
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509417/be689e54-2b6d-11e7-94d4-dd3d07f9f7d2.png)
            
  - Once, Playmaker and SWS are setup we can reference the Ped Behavior templates into the Ped_FSMTemplate Pool in the Path Generator       script attached to the SceneManager gameObject.
  
  - You can add references for the premade Ped behavior models/FSMs into the Path Generator script on the SceneManager gameObject.
  
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509629/3f78f98e-2b6f-11e7-85d2-1f55c0c48c56.png)
    
   - Next you can add the references for Ped prefabs and a prebuilt Path made of 2 waypoints.   
   
    ![image](https://cloud.githubusercontent.com/assets/23564961/25509669/890ea242-2b6f-11e7-91f6-662cc260b4d7.png)    
    
    - The combination of Ped behavior models and type of path can be selected from the PathTextFile boolean list and Ped FSM_template         Pool boolean list from the Path Generator script
    
    
    

