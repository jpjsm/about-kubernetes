# Pod-to-Pod vs Sidecar communications

## Overview

Learn how to deploy containers in kubernetes as pod-to-pod communications,
where one pod should be invisible from the external world.

Learn how to deploy containers where one pod works as a sidecar for the
other port.

In this examples, I'm going to use [frontapi](https://hub.docker.com/repository/docker/jpjofresm/frontapi/general)
and [backapi](https://hub.docker.com/repository/docker/jpjofresm/backapi/general)
containers; as they work together.

## Activities

- Deploy *backapi* with nodeport service to have a simple connection mechanism.

    1. `kubectl apply -f backapi-deployment.yaml`
    1. `kubectl apply -f backapi-nodeport-svc.yaml`
    1. `curl "http://<kubernetes-cluster-address>:32767/multiply?a=2&b=3"`
        > 6

- Deploy *fronapi* with nodeport service to have a simple connection mechanism.

    1. `kubectl apply -f frontapi-deployment.yaml`
    1. `kubectl apply -f frontapi-nodeport-svc.yaml`
    1. `curl "http://<kubernetes-cluster-address>:32766/?expression=2*3"`
        > 6

- Remove Pod-to-Pod demo

    1. `kubectl delete -f frontapi-deployment.yaml`
    1. `kubectl delete -f backapi-deployment.yaml`
    1. `kubectl delete -f frontapi-nodeport-svc.yaml`
    1. `kubectl delete -f backapi-nodeport-svc.yaml`
    1. `kubectl get all`

- Deploy *fronapi-with-sidecar* with nodeport service to have a simple connection mechanism.

    1. `kubectl apply -f frontapi-with-sidecar-deployment.yaml`
    1. `kubectl apply -f frontapi-with-sidecar-nodeport-svc.yaml`
    1. `curl "http://<kubernetes-cluster-address>:32765/?expression=2*3"`
        > 6
