
Requirements:
---------------------
python 3.9.x<br>
Unity 2022.3.10f1

Commands to run in project directory:
-------------------------------------
`py -3.9 -m venv venv`
<br>Creates a new virtual environment<br>

Activate the virtual environment:<br>
Windows:&emsp; `venv\Scripts\activate`<br>
&emsp;&ensp;&nbsp;Linux:&nbsp;&emsp;`./venv/Scripts/activate`

```
python -m pip install --upgrade pip
pip install mlagents 
pip3 install torch torchvision torchaudio
pip install protobuf==3.20.3 onnx 
```

Open Unity project (Unity version 2022.3.10f1)
----------------------------------------------
Load scene at $(SourceDir)/Assets/scenes/FinalShortLevel


Reinforcement Learning
------------------------
In the virtual enironment, run the command <br>
`mlagents-learn Assets\Behaviors\Reinforcement.yaml --run-id=reinforcement --force`<br>

Allow training to run for as long as wanted/needed

Imitation Learning
-------------------------
Run the following commands in order in the virual environment<br>

`mlagents-learn Assets\Behaviors\Imitation.yaml --run-id=imitation --force`<br>

Wait for completion<br>

`mlagents-learn Assets\Behaviors\Imitation2.yaml --run-id=imitation --resume`<br>

Wait for completion<br>

`mlagents-learn Assets\Behaviors\Imitation3.yaml --run-id=imitation --resume`<br>

Wait for completion<br>

`mlagents-learn Assets\Behaviors\Imitation4.yaml --run-id=imitation --resume`<br>

Allow training for as long as needed/wanted


Curriculum Learning
----------------------
Run the following command in the virtual environment <br>
`mlagents-learn Assets\Behaviors\Curriculum.yaml --run-id=curriculum --force`<br>



Visualizing Results
--------------------

To visualize results, in venv run:<br>
`tensorboard --logdir results\`<br>
and go to web address <a href="localhost:6066">`localhost:6006`</a>