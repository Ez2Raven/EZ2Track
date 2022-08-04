---
title: "Let's refresh and baseline EZ2Track development"
excerpt_separator: "<!--more-->"
categories: "Blog"
tags: "Software Engineering"
toc: true
---

# Enforcing Development and Engineering practises

In my previous development gig, I came to realize the current supply of developers within my region
can no longer fulfil the demand for good C# engineering practises.

Thus I am currently working on means to enforce good software engineering practices
within the boundaries of coding.

AFAIK, there are already practises in the FOSS community for the above. However it is unclear if
such practices are well defined in the working industry.

<!--more-->

# What exactly is within the boundaries of coding?

A few topics are shortlisted that can help developers to be more 'agile' when it comes to software
engineering practices.

- Documentation as Code
    - [Markdown](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)
    - [Jekyllrb](https://jekyllrb.com) generates html from markdown files
- Diagram as Code
    - [Diagrams, python package that generate diagrams](https://github.com/mingrammer/diagrams)
    <figure>
      <img src="{{ '/assets/images/ez2crawl_overview.png' | relative_url }}" alt="image generated using diagrams">
    </figure>
- Infrastructure as Code
    - Terraform
    - docker
    - docker-compose, kubernetes and equivalents
  > We can probably categorize this as 'infrastructure as code', since we are using yaml to
  > define how containers interacts within its hosting platform.

# So what's next?

For now, I just want to focus on enforcing a consistent development experience for developers.
It will begin after a User Story is articulated and an initial design is agreed within the
engineering team.

_i.e._

- Documentation as Code
  > Changes to the product and solution design should reflect code changes.
- Diagram as code
  > Stop adjusting diagram blocks by hand and source control it.
- Infrastructure as Code
  > I do not foresee EZ2Track encountering huge user traffic,
  > so it will be hosted as docker containers in the simplest platform.
