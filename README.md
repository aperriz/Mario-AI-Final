Requirements:

python 3.9.x
Unity 2022.3.10f1

Commands to run in project directory:

py -3.9 -m venv venv

(windows) venv\Scripts\activate

python -m pip install --upgrade pip
pip install mlagents 
pip3 install torch torchvision torchaudio
pip install protobuf==3.20.3 onnx

(after opening unity project)
Load assets/scenes/FinalLevel


mlagents-learn Assets\Behaviors\Imitation.yaml --run-id=imitationLL --force

Wait for completion

mlagents-learn Assets\Behaviors\Imitation2.yaml --run-id=imitationLL --resume

Wait for completion

mlagents-learn Assets\Behaviors\Reinforcement.yaml --run-id=imitationLL --resume

Continue training for as long as wanted/needed



To visualize results, in venv run:
tensorboard --logdir results\
and go to localhost:6006