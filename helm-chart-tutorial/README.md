# Helm Chart Tutorial

Source: [Getting Started](https://helm.sh/docs/chart_template_guide/getting_started/)

## Steps

- `helm create mychart` # creates the basic structure of a Helm chart

- remove all files from **\*/templates/**  
  `rm -rf mychart/templates/*`

- Add configmap.yaml

- `helm install foo-bar ./mychart`

- Update configmap.yaml, add a *template directive* `{{` <namespaced.element> `}}`, i.e. `{{ .Release.Name }}`

>> ***Note***:  
When you want to test the template rendering, but not actually install anything, you can use `helm install --debug --dry-run <chart-name> <source>`  
I.e. `helm install --debug --dry-run goodly-guppy ./mychart`
