name: Feature Request
description: Suggest an idea for City Sim
title: "[FEATURE] "
labels: ["enhancement"]

body:
  - type: markdown
    attributes:
      value: Thanks for suggesting a feature!

  - type: textarea
    id: description
    attributes:
      label: Description
      description: Describe the feature you'd like
    validations:
      required: true

  - type: textarea
    id: motivation
    attributes:
      label: Motivation
      description: Why is this feature needed? What problem does it solve?
    validations:
      required: true

  - type: textarea
    id: implementation
    attributes:
      label: Possible Implementation
      description: How could this be implemented?

  - type: dropdown
    id: priority
    attributes:
      label: Priority
      options:
        - Low
        - Medium
        - High
    validations:
      required: true
