# Contributing

- No LLM PRs please. I know "this is how the industry is moving" but I do not care. This is a hobby and learning project. Work with your hands and your own brain. Embrace the friction.

- Do not use Harmony Patches for anything outside of the following
  - Structural changes
     - The original engine has no way to check time remaining in a game without constant polling. Patches were used to allow it to run off of a Timer that would raise an event when someone ran out
  - Avoiding Unity Landmines
    - Some feautures only work under unity. For example, the card Pippit Hustler calls Unity specific code and will crash the server. We use a patch to avoid those calls.
