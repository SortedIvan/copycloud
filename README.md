# copycloud
Copycloud is a real time collaboration tool that allows marketing teams and professionals to utilize OpenAI and other popular tool integrations in a real time collaboration environment.

Copycloud was a project revolved around learning enterprise software. It focused on creating scalable architecture (by using micro-services), utilizing cloud services for deployment, authentication, nosql storage and orchestration (kubernetes). This document includes some of the most important highlights of the application, as well as how the application looks like in a local development environment. It was formerly deployed and running by using Netlify and Azure Kubernetes Service, but is currently stopped due to changes in the application.

<h1> Architecture Diagram </h1>
<p> Copycloud uses a microservice architecture, where each unit of logic is split to a seperate service in order to make scalability and multiple deployments much more accessible. Furthemore, this allows for a higher level application of the SOLID principles and de-couples logic, improving code quality.</p>

- In the below diagram, we can see 4 microservices, 2 of which have a NoSQL database and 2 being background services. All of the communication between services is done with the help of events and message queues.

![ArchitectureDiagram](https://github.com/SortedIvan/copycloud/assets/62967263/b1fe054b-2a76-4741-9125-a19850f80923)

<h1> Deployment & scalability</h1>
<p> The application was deployed using Azure kubernetes service, which allows for different services of the application to be scaled manually or to be specified a range of how many automatic instances should be spinned up in the case of high traffic. This is particularly important for services that have to deal with users (like the userservice) and documents within the application (documentservice). The pictures below highlight the most important parts of the deployment</p>

<h4>Workloads and services in Azure</h4>

![DeploymentService](https://github.com/SortedIvan/copycloud/assets/62967263/267f5833-31b0-443d-a513-b1ab7b916186)

![Cluster services](https://github.com/SortedIvan/copycloud/assets/62967263/9bffbf2a-8a02-4d50-8e04-fde13357baa9)
