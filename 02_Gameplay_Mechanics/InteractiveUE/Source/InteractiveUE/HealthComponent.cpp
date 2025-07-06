// Fill out your copyright notice in the Description page of Project Settings.


#include "HealthComponent.h"

// Sets default values for this component's properties
UHealthComponent::UHealthComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;

	// ...
}


// Called when the game starts
void UHealthComponent::BeginPlay()
{
	Super::BeginPlay();

	CurrentHealth = MaxHealth;
	ShowRed = false;
	// ...
	
}


// Called every frame
void UHealthComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...
}

void UHealthComponent::TakeDamage(float DamageAmount)
{
	if (CurrentHealth <= 0 || IsDead()) { return; }
	
	float CurrentTime = GetWorld()->GetTimeSeconds();

	if (CurrentTime - LastDamageTime < RedFlashTimer)
	{
		ShowRed = true;
		
	}
	
	LastDamageTime = CurrentTime;
	ShowRed = true;
	CurrentHealth = FMath::Clamp(CurrentHealth - DamageAmount, 0.0f, MaxHealth);

	GetWorld()->GetTimerManager().ClearTimer(RedFlashTimerHandle); // in case one is already running
	GetWorld()->GetTimerManager().SetTimer(RedFlashTimerHandle, this, &UHealthComponent::HideRedFlash, 0.3f, false);


	OnHealthChanged.Broadcast();

	if (IsDead()) {
		OnDeath();
	}
}

void UHealthComponent::AddHealth(float AddedHealth)
{
	if (CurrentHealth + AddedHealth >= MaxHealth) {
		CurrentHealth = MaxHealth;
	}
	else {
		CurrentHealth += AddedHealth;
	}
}

void UHealthComponent::OnDeath()
{
	OnDeathEvent.Broadcast();
}

void UHealthComponent::HideRedFlash()
{
	ShowRed = false;
}


