# Part 7 — Kubernetes Demo Commands
## PROG3176 Final Project — VehicleTrack API

Run these commands one at a time in order.

---

## Step 1: Start Minikube
```
minikube start
```

## Step 2: Point Docker to Minikube's registry (so it finds the local image)
```
minikube docker-env
```
On Windows Git Bash, run the output command. It will look like:
```
eval $(minikube -p minikube docker-env)
```

## Step 3: Build the Docker image inside Minikube
```
cd VehicleTrackAPI
docker build -t vehicletrack-api:latest .
```

## Step 4: Apply the Kubernetes manifests
```
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
```

## Step 5: Verify pods are running (screenshot this)
```
kubectl get pods
```
Expected output: 2 pods with STATUS = Running

## Step 6: Get the NodePort URL
```
minikube service vehicletrack-service --url
```
Open that URL + `/swagger` in your browser (screenshot this)

## Step 7: Scale to 5 replicas (screenshot before and after)
```
kubectl get pods
kubectl scale deployment vehicletrack-api --replicas=5
kubectl get pods
```

## Step 8: Demonstrate self-healing (screenshot)
```
kubectl get pods
kubectl delete pod <paste-one-pod-name-here>
kubectl get pods
```
A new pod will automatically recreate within seconds.

## Step 9: Clean up when done
```
kubectl delete -f k8s/deployment.yaml
kubectl delete -f k8s/service.yaml
minikube stop
```
