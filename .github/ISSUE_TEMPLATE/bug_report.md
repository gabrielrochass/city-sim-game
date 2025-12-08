name: Bug Report
description: Report a bug found in City Sim
title: "[BUG] "
labels: ["bug"]

body:
  - type: markdown
    attributes:
      value: |
        Thanks for taking the time to fill out this bug report!

  - type: input
    id: unity_version
    attributes:
      label: Unity Version
      description: What version of Unity are you using?
      placeholder: 2022.3.0f1
    validations:
      required: true

  - type: input
    id: game_version
    attributes:
      label: Game Version
      description: What version of City Sim are you running?
      placeholder: 1.0.0
    validations:
      required: true

  - type: textarea
    id: description
    attributes:
      label: Description
      description: A clear description of what the bug is
    validations:
      required: true

  - type: textarea
    id: repro
    attributes:
      label: Steps to Reproduce
      description: Steps to reproduce the behavior
      placeholder: |
        1. Open game
        2. Click on...
        3. See error
    validations:
      required: true

  - type: textarea
    id: expected
    attributes:
      label: Expected Behavior
      description: What should happen instead?
    validations:
      required: true

  - type: textarea
    id: logs
    attributes:
      label: Console Logs
      description: Any error messages from Console
      render: shell

  - type: textarea
    id: screenshots
    attributes:
      label: Screenshots
      description: If applicable, add screenshots

  - type: dropdown
    id: priority
    attributes:
      label: Priority
      options:
        - Low
        - Medium
        - High
        - Critical
    validations:
      required: true
